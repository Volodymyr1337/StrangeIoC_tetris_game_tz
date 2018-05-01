using System;
using strange.framework.api;
using UnityEngine;

namespace strange.framework.api
{
    public class ResourceInstanceProvider : IInstanceProvider
    {
        GameObject prototype;
        
        private string pathToResource;

        private int layer;

        private int id = 0;

        public ResourceInstanceProvider(string resourcePath, int layer)
        {
            pathToResource = resourcePath;
            this.layer = layer;
        }
        
        public T GetInstance<T>()
        {
            object instance = GetInstance(typeof(T));
            T retv = (T)instance;
            return retv;
        }
        
        public object GetInstance(Type key)
        {
            if (prototype == null)
            {
                prototype = Resources.Load<GameObject>(pathToResource);
            }
            
            GameObject go = GameObject.Instantiate(prototype) as GameObject;
            go.transform.localScale = Vector3.one;
            go.layer = layer;
            go.name = prototype.name + "_" + id++;
            return go;
        }
    }
}

