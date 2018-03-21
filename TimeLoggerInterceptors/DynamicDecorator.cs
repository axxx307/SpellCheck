
using System.IO;
using System.Linq;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace spell_check
{
    public class DynamicDecorator<TComponentClass> : DynamicObject, IInterceptor
    {
        /// <summary>Component - defines an object to which
        /// additional responsibilities can be attached.</summary>
        protected TComponentClass _component;

        /// <summary>Represents component type.</summary>
        private Type _componentType;

        /// <summary>Initializes a new instance of the
        /// <see cref="DynamicDecorator&lt;TComponentClass&gt;"/> class.</summary>
        /// <param name="component">The component to which additional
        /// responsibilities can be attached.</param>
        public DynamicDecorator(TComponentClass component)
        {
            Component = component;

            var _generator = new ProxyGenerator();
            Class = (TComponentClass)_generator.CreateClassProxy(
                typeof(TComponentClass), this);
        }

        /// <summary>Gets or sets the object to which additional
        /// responsibilities can be attached.</summary>
        public TComponentClass Component
        {
            get { return _component; }
            set
            {
                _component = value;
                _componentType = _component.GetType();
            }
        }

        /// <summary>Gets the component Class.</summary>
        /// <value>The dynamic proxy used to implement the TComponent Class.</value>
        public TComponentClass Class { get; private set; }

        #region DynamicObject overrides

        /// <summary>Provides the implementation for operations that set member values.</summary>
        /// <param name="binder">Provides information about the object
        /// that called the dynamic operation.</param>
        /// <param name="value">The value to set to the member.</param>
        /// <returns>true if the operation is successful; otherwise, false.</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // search for a property
            var property = _componentType.GetProperty(
                binder.Name, BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                property.SetValue(_component, value, null);
                return true;
            }

            // search for a public field
            var field = _componentType.GetField(
                binder.Name, BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(_component, value);
                return true;
            }

            return base.TrySetMember(binder, value);
        }

        /// <summary>Provides the implementation for operations that get member values.</summary>
        /// <param name="binder">Provides information about the object
        /// that called the dynamic operation.</param>
        /// <param name="result">The result of the get operation.</param>
        /// <returns>true if the operation is successful; otherwise, false.</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // search for a property
            var property = _componentType.GetProperty(
                binder.Name, BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                result = property.GetValue(_component, null);
                return true;
            }

            // search for a public field
            var field = _componentType.GetField(
                binder.Name, BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                result = field.GetValue(_component);
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        /// <summary>Provides the implementation for operations that invoke a member.</summary>
        /// <param name="binder">Provides information about the dynamic operation.</param>
        /// <param name="args">The arguments that are passed to the object member
        /// during the invoke operation.</param>
        /// <param name="result">The result of the member invocation.</param>
        /// <returns>true if the operation is successful; otherwise, false.</returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args,
            out object result)
        {
            var method = _componentType.GetMethod(
                binder.Name, BindingFlags.Public | BindingFlags.Instance,
                null, GetArgumentsTypes(args), null);

            if (method != null)
            {
                result = method.Invoke(_component, args);
                return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        #endregion

        #region Castle IInterceptor

        /// <summary>Intercepts the specified invocation.</summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            var decoratorType = GetType();
            var argTypes = invocation.Method.GetParameters().Select(x => x.ParameterType).ToArray();

            var method = decoratorType.GetMethod(
                invocation.Method.Name, BindingFlags.Public | BindingFlags.Instance,
                null, argTypes, null);

            if (method != null)
            {
                invocation.ReturnValue = method.Invoke(this, invocation.Arguments);
                return;
            }

            method = _componentType.GetMethod(
                invocation.Method.Name, BindingFlags.Public | BindingFlags.Instance,
                null, argTypes, null);

            if (method != null)
            {
                invocation.ReturnValue = method.Invoke(_component, invocation.Arguments);
                return;
            }
        }

        #endregion

        /// <summary>Gets the component.</summary>
        /// <returns>The component.</returns>
        public TComponentClass GetComponent()
        {
            var decorator = Component as DynamicDecorator<TComponentClass>;
            if (decorator != null) return decorator.GetComponent();

            return Component;
        }

        /// <summary>Sets the component.</summary>
        /// <param name="component">The component to set.</param>
        public void SetComponent(TComponentClass component)
        {
            var decorator = Component as DynamicDecorator<TComponentClass>;
            if (decorator != null)
            {
                decorator.SetComponent(component);
                return;
            }

            Component = component;
        }

        /// <summary>Gets the list of arguments types.</summary>
        /// <param name="args">An object array that contains arguments.</param>
        /// <returns>The list of arguments types.</returns>
        private Type[] GetArgumentsTypes(object[] args)
        {
            var argTypes = new Type[args.GetLength(0)];

            var index = 0;
            foreach (var arg in args)
            {
                argTypes[index] = arg?.GetType();
                index++;
            }

            return argTypes;
        }

        /// <summary>Gets the list of arguments types as string.</summary>
        /// <param name="args">An object array that contains arguments.</param>
        /// <returns>The list of arguments types as string.</returns>
        private string GetArgumentsString(object[] args)
        {
            var argList = string.Empty;
            var isFirstArgument = true;

            foreach (var arg in args)
            {
                if (isFirstArgument)
                    isFirstArgument = false;
                else
                    argList += ", ";

                argList += arg.GetType().ToString();
            }

            return argList;
        }
    }
}