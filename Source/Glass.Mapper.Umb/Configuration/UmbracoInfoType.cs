﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Umb.Configuration
{
    public enum UmbracoInfoType
    {
        /// <summary>
        /// No value has been set
        /// </summary>
        NotSet,
        /// <summary>
        /// The item's path. The property type must be System.String
        /// </summary>
        Path,
        /// <summary>
        /// The item's Content Type Alias. The property type must be System.Guid
        /// </summary>
        ContentTypeAlias,
        /// <summary>
        /// The item's Content Type name. The property type must be System.String
        /// </summary>
        ContentTypeName,
        /// <summary>
        /// The item's URL. The property type must be System.String
        /// </summary>
        Url,
        /// <summary>
        /// The item's version. The property type must be System.Int32
        /// </summary>
        Version,
        /// <summary>
        /// The item's Name. The property type must be System.String
        /// </summary>
        Name
    }
}
