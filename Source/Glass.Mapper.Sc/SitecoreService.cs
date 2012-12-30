﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class SitecoreService : AbstractService<SitecoreDataMappingContext>, ISitecoreService
    {
        public  Database Database { get; private set; }

        public SitecoreService(Database database, string contextName = "Default")
            :base(contextName)
        {
            Database = database;
        }

        public SitecoreService(string databaseName, string contextName = "Default")
            : base(contextName)
        {
            Database = Sitecore.Configuration.Factory.GetDatabase(databaseName);
        }

        public SitecoreService(string databaseName, Context context)
            : base(context ?? Context.Default )
        {
            Database = Sitecore.Configuration.Factory.GetDatabase(databaseName);
        }

        public SitecoreService(Database database, Context context)
            : base(context ?? Context.Default)
        {
            Database = database;
        }


        public T GetItem<T>(Guid id) where T:class 
        {
            var item = Database.GetItem(new ID(id));
            return CreateClass(typeof (T), item) as T;

        }

        public void Save<T>(T obj)
        {
            //TODO: should this be a separate context
          //  SitecoreTypeContext context = new SitecoreTypeContext();

            //TODO: ME - this may not work with a proxy
            var config = GlassContext[obj.GetType()] as SitecoreTypeConfiguration;
            var item = config.ResolveItem(obj, Database);
            if(item == null)
                throw new MapperException("Could not save class, item not found");

            SitecoreTypeSavingContext savingContext = new SitecoreTypeSavingContext();
            savingContext.Config = config;

            //ME-an item with no versions should be null

            savingContext.Item = item == null || item.Versions.Count == 0 ? null : item;
            savingContext.Object = obj;

            item.Editing.BeginEdit();

            SaveObject(savingContext);

            item.Editing.EndEdit();
        }

        public object CreateClass(Type type, Item item, bool isLazy = false, bool inferType = false)
        {
            if (item == null) return null;

            SitecoreTypeCreationContext creationContext = new SitecoreTypeCreationContext();
            creationContext.SitecoreService = this;
            creationContext.RequestedType = type;
            creationContext.ConstructorParameters = null;
            creationContext.Item = item;
            creationContext.InferType = inferType;
            creationContext.IsLazy = isLazy;
            var obj = InstantiateObject(creationContext);

            return obj;
        }

        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext abstractTypeCreationContext, Object obj)
        {
            var scTypeContext = abstractTypeCreationContext as SitecoreTypeCreationContext;
            return new SitecoreDataMappingContext(obj, scTypeContext.Item, this);
        }

        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext)
        {
            var scContext = creationContext as SitecoreTypeSavingContext;
            return new SitecoreDataMappingContext(scContext.Object, scContext.Item, this);
        }

        
    }
}
