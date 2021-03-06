//note: I don't know who is responsible for writing out the most of this excellent stuff
//note: If you feel you are somehow involved and not mentioned in credits - let me know
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace LogoFX.Client.Core
{
    /// <summary>
    /// Implements <see cref="INotifyPropertyChanged"/> on behalf of a container class.
    /// </summary>
    /// <remarks>
    /// <para>Use <see cref="NotifyPropertyChangedBase{TObject}"/> instead of this class if possible.</para>
    /// </remarks>
    /// <typeparam name="T">The type of the containing class.</typeparam>
    public sealed class NotifyPropertyChangedCore<T>
    {
        /// <summary>
        /// The backing delegate for <see cref="PropertyChanged"/>.
        /// </summary>
        private PropertyChangedEventHandler _propertyChanged;

        /// <summary>
        /// The object that contains this instance.
        /// </summary>
        private readonly T _obj;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyPropertyChangedCore{T}"/> class that is contained by <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object that contains this instance.</param>
        public NotifyPropertyChangedCore(T obj)
        {
            _obj = obj;
        }

        /// <summary>
        /// Provides notification of changes to a property value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _propertyChanged += value;
            }

            remove
            {
                _propertyChanged -= value;
            }
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        public void OnPropertyChanged<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            _propertyChanged.Raise(_obj, expression);
        }
        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        public void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> expression)
        {
            _propertyChanged.Raise(_obj, expression);
        }
    }
}
