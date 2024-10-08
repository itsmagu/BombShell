﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Qommon;

namespace Laylua.Library.Entities.Reference.Table;

public unsafe partial class LuaTable
{
    /// <summary>
    ///     Represents a view over the values of a <see cref="LuaTable"/>.
    /// </summary>
    public readonly struct ValueCollection
    {
        private readonly LuaTable _table;

        internal ValueCollection(LuaTable table)
        {
            _table = table;
        }

        /// <summary>
        ///     Gets an array containing the values of the table.
        /// </summary>
        /// <remarks>
        ///     <inheritdoc cref="ToList{T}"/>
        /// </remarks>
        /// <param name="throwOnNonConvertible"> Whether to throw on non-convertible values. </param>
        /// <returns>
        ///     The output array.
        /// </returns>
        public T[] ToArray<T>(bool throwOnNonConvertible = true)
            where T : notnull
        {
            var list = ToList<T>(throwOnNonConvertible);
            return list.ToArray();
        }

        /// <summary>
        ///     Gets a list containing the values of the table.
        /// </summary>
        /// <remarks>
        ///     This method throws for values that cannot be converted to <typeparamref name="T"/>
        ///     or skips them if <paramref name="throwOnNonConvertible"/> is <see langword="false"/>.
        /// </remarks>
        /// <param name="throwOnNonConvertible"> Whether to throw on non-convertible values. </param>
        /// <returns>
        ///     The output list.
        /// </returns>
        public List<T> ToList<T>(bool throwOnNonConvertible = true)
            where T : notnull
        {
            var lua = _table.Lua;
            lua.Stack.EnsureFreeCapacity(3);

            using (_table.Lua.Stack.SnapshotCount())
            {
                PushValue(_table);
                var L = lua.GetStatePointer();
                var marshaler = lua.Marshaler;
                var length = (int) luaL_len(L, -1);
                var list = new List<T>(Math.Min(length, 256));
                lua_pushnil(L);
                while (lua_next(L, -2))
                {
                    if (!marshaler.TryGetValue<T>(-1, out var value))
                    {
                        if (throwOnNonConvertible)
                        {
                            Throw.InvalidOperationException($"Failed to convert the value {lua.Stack[-1]} to type {typeof(T)}.");
                        }
                    }
                    else
                    {
                        Debug.Assert(value != null);
                        list.Add(value);
                    }

                    lua_pop(L);
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets an enumerable lazily yielding the values of the table.
        /// </summary>
        /// <remarks>
        ///     This method throws for values that cannot be converted to <typeparamref name="T"/>
        ///     or skips them if <paramref name="throwOnNonConvertible"/> is <see langword="false"/>.
        /// </remarks>
        /// <param name="throwOnNonConvertible"> Whether to throw on non-convertible values. </param>
        /// <returns>
        ///     The output enumerable.
        /// </returns>
        public IEnumerable<T> ToEnumerable<T>(bool throwOnNonConvertible = true)
            where T : notnull
        {
            foreach (var stackValue in this)
            {
                if (!stackValue.TryGetValue<T>(out var value))
                {
                    if (throwOnNonConvertible)
                    {
                        Throw.InvalidOperationException($"Failed to convert the value {stackValue} to type {typeof(T)}.");
                    }
                }

                yield return value!;
            }
        }

        /// <summary>
        ///     Returns an enumerator that enumerates values of the table.
        /// </summary>
        /// <remarks>
        ///     The enumerator uses the Lua stack which you should
        ///     keep in mind when pushing your own data onto the stack during enumeration.
        /// </remarks>
        /// <returns>
        ///     An enumerator wrapping the values of the table.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            return new(_table);
        }

        /// <summary>
        ///     Represents an enumerator that can be used to enumerate the values of a <see cref="LuaTable"/>.
        /// </summary>
        public struct Enumerator : IEnumerator<LuaStackValue>
        {
            /// <inheritdoc/>
            public readonly LuaStackValue Current
            {
                get
                {
                    if (_moveTop == 0)
                        return default;

                    return _table.Lua.Stack[-1];
                }
            }

            object IEnumerator.Current => Current;

            private readonly LuaTable _table;
            private readonly int _initialTop;
            private int _moveTop;

            internal Enumerator(LuaTable table)
            {
                table.ThrowIfInvalid();

                _table = table;
                var L = _table.Lua.GetStatePointer();
                _initialTop = lua_gettop(L);
                _moveTop = 0;

                table.Lua.Stack.EnsureFreeCapacity(2);

                try
                {
                    PushValue(_table);
                    lua_pushnil(L);
                }
                catch
                {
                    lua_settop(L, _initialTop);
                    throw;
                }
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                var L = _table.Lua.GetStatePointer();
                if (_moveTop != 0)
                {
                    lua_settop(L, _moveTop);
                    _moveTop = 0;
                }

                if (!lua_next(L, -2))
                    return false;

                _moveTop = _initialTop + 2;
                return true;
            }

            /// <inheritdoc/>
            public void Reset()
            {
                Dispose();
                this = new Enumerator(_table);
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                _moveTop = 0;
                var L = _table.Lua.GetStatePointer();
                lua_settop(L, _initialTop);
            }
        }
    }
}
