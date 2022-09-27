//-----------------------------------------------------------------------------
// <copyright file="NavigationTemplateSegment.cs" company=".NET Foundation">
//      Copyright (c) .NET Foundation and Contributors. All rights reserved.
//      See License.txt in the project root for license information.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nox.Dynamic.OData.Routing
{
    public class NavigationTemplateSegment : ODataSegmentTemplate
    {
        public override IEnumerable<string> GetTemplates(ODataRouteOptions options)
        {
            yield return "/{navigation}";
        }

        public override bool TryTranslate(ODataTemplateTranslateContext context)
        {
            if (!context.RouteValues.TryGetValue("navigation", out object? navigationNameObj))
            {
                return false;
            }

            var navigationName = navigationNameObj as string;
            var keySegment = context.Segments.Last() as KeySegment;
            var entityType = keySegment?.EdmType as IEdmEntityType;

            var navigationProperty = entityType
                .NavigationProperties()
                .FirstOrDefault(n => n.Name.Equals(navigationName,StringComparison.OrdinalIgnoreCase));

            if (navigationProperty != null)
            {
                var navigationSource = keySegment?.NavigationSource;
                var targetNavigationSource = navigationSource?.FindNavigationTarget(navigationProperty);

                var seg = new NavigationPropertySegment(navigationProperty, targetNavigationSource);
                context.Segments.Add(seg);
                return true;
            }

            return false;
        }
    }
}