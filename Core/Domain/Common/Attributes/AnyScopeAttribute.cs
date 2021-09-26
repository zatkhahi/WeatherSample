using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AnyScopeAttribute : Attribute
    {
        public string[] Scopes { get; private set; }
        // public bool HasAuthorizeHandler { get; set; } = false;


        public AnyScopeAttribute() { }
        public AnyScopeAttribute(params string[] scopes)
        {
            Scopes = scopes;
        }

        public ScopeProperty[] GetScopes()
        {
            if (Scopes == null)
                return new ScopeProperty[0];

            var scopes = new List<ScopeProperty>();
            foreach (var scope in Scopes)
            {
                string scopeName;
                string propertyName = null;
                if (scope.Contains(":"))
                {
                    var scopeAndProperty = scope.Split(":");
                    if (scopeAndProperty.Length != 2)
                        throw new Exception($"Invalid Scope '{scope}' definition for type");
                    scopeName = scopeAndProperty[0];
                    propertyName = scopeAndProperty[1];
                }
                else
                {
                    scopeName = scope;
                }
                scopes.Add(new ScopeProperty()
                {
                    Scope = scopeName,
                    PropertyName = propertyName
                });
            }
            return scopes.ToArray();
        }

    }
}
