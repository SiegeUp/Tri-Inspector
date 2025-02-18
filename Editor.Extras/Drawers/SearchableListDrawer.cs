using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using TriInspector.Drawers;
using TriInspector.Elements;
using TriInspector.Utilities;
using TriInspectorUnityInternalBridge;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[assembly: RegisterTriAttributeDrawer(typeof(SearchableListDrawer), TriDrawerOrder.Drawer)]

namespace TriInspector.Drawers
{
    public class SearchableListDrawer : TriAttributeDrawer<SearchableAttribute>
    {
        public override TriExtensionInitializationResult Initialize(TriPropertyDefinition propertyDefinition)
        {
            if (!propertyDefinition.IsArray)
            {
                return "[Searchable] valid only on lists";
            }

            // Если у поля есть [TableList], пропускаем этот Drawer
            if (propertyDefinition.Attributes.Any(i => i.GetType() == typeof(TableListAttribute)))
            {
                return TriExtensionInitializationResult.Skip;
            }

            return TriExtensionInitializationResult.Ok;
        }

        public override TriElement CreateElement(TriProperty property, TriElement next)
        {
            return new SearchableListElement(property);
        }

        private class SearchableListElement : TriListElement
        {
            private const float FooterExtraSpace = 4f; // Отступ, чтобы футер не обрезался

            private readonly TriProperty _property;
            private string _searchText = "";
            private List<int> _filteredIndices = new List<int>();

            public SearchableListElement(TriProperty property) : base(property)
            {
                _property = property;
            }

            public override bool Update()
            {
                // Стандартное обновление списка
                bool dirty = base.Update();

                // Пересчитываем отфильтрованные индексы
                _filteredIndices = SearchableUtils.FilterIndices(_property.ArrayElementProperties, _searchText);

                return dirty;
            }

            public override float GetHeight(float width)
            {
                float height = 0f;

                // 1) Высота заголовка списка
                height += ListGui.headerHeight;

                // 2) Если список не развернут, завершаем расчёт
                if (!_property.IsExpanded)
                {
                    return height;
                }

                // 3) Если список развернут, добавляем высоту поля поиска
                height += EditorGUIUtility.singleLineHeight;

                // 4) Если строка поиска пуста, используем стандартный расчёт TriListElement
                if (string.IsNullOrEmpty(_searchText))
                {
                    height += base.GetHeight(width);
                    // Добавляем небольшой отступ под футер
                    height += FooterExtraSpace;
                    return height;
                }

                // 5) Иначе считаем высоту вручную
                //    - высота всех отфильтрованных элементов
                //    - высота футера
                for (int i = 0; i < _filteredIndices.Count; i++)
                {
                    int idx = _filteredIndices[i];
                    if (idx < ChildrenCount)
                    {
                        height += GetChild(idx).GetHeight(width);
                    }
                }

                height += ListGui.footerHeight;
                height += FooterExtraSpace;

                return height;
            }

            public override void OnGUI(Rect position)
            {
                // 1) Рисуем заголовок списка
                var headerRect = new Rect(position.x, position.y, position.width, ListGui.headerHeight);
                ReorderableListProxy.DoListHeader(ListGui, headerRect);

                // Сдвигаем область для отрисовки вниз
                float yPos = headerRect.yMax;

                // Если список не развернут, больше ничего не рисуем
                if (!_property.IsExpanded)
                {
                    return;
                }

                // 2) Поле поиска (сразу под заголовком)
                var searchRect = new Rect(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight);
                _searchText = EditorGUI.TextField(searchRect, _searchText, EditorStyles.toolbarSearchField);
                yPos += EditorGUIUtility.singleLineHeight;

                // 3) Основная область списка (после поля поиска)
                var listRect = new Rect(position.x, yPos, position.width, position.height - (yPos - position.y));

                // Если строка поиска пуста — стандартное поведение TriListElement
                if (string.IsNullOrEmpty(_searchText))
                {
                    base.OnGUI(listRect);
                    return;
                }

                // Иначе — отключаем перетаскивание и вручную рисуем элементы
                ListGui.draggable = false;

                // Заголовок уже отрисован, поэтому начинаем сразу с контента
                float contentY = listRect.y;

                // Отрисовываем отфильтрованные элементы
                foreach (var idx in _filteredIndices)
                {
                    if (idx < ChildrenCount)
                    {
                        var child = GetChild(idx);
                        float elementHeight = child.GetHeight(listRect.width);

                        var elementRect = new Rect(listRect.x, contentY, listRect.width, elementHeight);
                        child.OnGUI(elementRect);

                        contentY += elementHeight;
                    }
                }

                // Подвал
                var footerRect = new Rect(listRect.x, contentY, listRect.width, ListGui.footerHeight);
                ReorderableListProxy.defaultBehaviours.DrawFooter(footerRect, ListGui);
            }
        }
    }
}
