using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Laylua.Library.Entities.Reference;
using Laylua.Library.Marshaler.UserData.Descriptor;
using Laylua.Library.Marshaler.UserData.Descriptor.Default.Handle;
using Laylua.Moon.Native.Extern;
using Qommon;

namespace Laylua.Library.Marshaler;

public unsafe partial class DefaultLuaMarshaler
{
    /// <inheritdoc/>
    [SkipLocalsInit]
    public override void PushValue<T>(T obj)
    {
        var L = Lua.GetStatePointer();
        switch (obj)
        {
            case null:
            {
                lua_pushnil(L);
                return;
            }
            case bool:
            {
                lua_pushboolean(L, (bool) (object) obj);
                return;
            }
            case sbyte:
            {
                lua_pushinteger(L, (sbyte) (object) obj);
                return;
            }
            case byte:
            {
                lua_pushinteger(L, (byte) (object) obj);
                return;
            }
            case short:
            {
                lua_pushinteger(L, (short) (object) obj);
                return;
            }
            case ushort:
            {
                lua_pushinteger(L, (ushort) (object) obj);
                return;
            }
            case int:
            {
                lua_pushinteger(L, (int) (object) obj);
                return;
            }
            case uint:
            {
                lua_pushinteger(L, (uint) (object) obj);
                return;
            }
            case long:
            {
                lua_pushinteger(L, (long) (object) obj);
                return;
            }
            case ulong:
            {
                lua_pushinteger(L, (long) (ulong) (object) obj);
                return;
            }
            case float:
            {
                lua_pushnumber(L, (float) (object) obj);
                return;
            }
            case double:
            {
                lua_pushnumber(L, (double) (object) obj);
                return;
            }
            case decimal:
            {
                lua_pushnumber(L, (lua_Number) (decimal) (object) obj);
                return;
            }
            case char:
            {
                var charValue = (char) (object) obj;
#if NET7_0_OR_GREATER
                lua_pushstring(L, new ReadOnlySpan<char>(in charValue));
#else
                lua_pushstring(L, MemoryMarshal.CreateReadOnlySpan(ref charValue, 1));
#endif
                return;
            }
            case string:
            {
                lua_pushstring(L, (string) (object) obj);
                return;
            }
            case ReadOnlyMemory<char>:
            {
                lua_pushstring(L, ((ReadOnlyMemory<char>) (object) obj).Span);
                return;
            }
            case IntPtr:
            {
                lua_pushlightuserdata(L, ((IntPtr) (object) obj).ToPointer());
                return;
            }
            case UIntPtr:
            {
                lua_pushlightuserdata(L, ((UIntPtr) (object) obj).ToPointer());
                return;
            }
            case LuaStackValue:
            {
                ((LuaStackValue) (object) obj).PushValue();
                return;
            }
            case LuaReference:
            {
                LuaReference.PushValue((LuaReference) (object) obj);
                return;
            }
            case UserDataHandle:
            {
                ((UserDataHandle) (object) obj).Push();
                return;
            }
            default:
            {
                if (obj is DescribedUserData)
                {
                    (obj as DescribedUserData)!.CreateUserDataHandle(Lua).Push();
                    return;
                }

                if (!typeof(T).IsValueType)
                {
                    if (UserDataDescriptorProvider.TryGetDescriptor<T>(obj, out var descriptor))
                    {
                        if (_userDataHandleCache.TryGetValue((obj, descriptor), out var handle))
                        {
                            handle.Push();
                            return;
                        }

                        handle = new UserDataHandle<T>(Lua, obj, descriptor);
                        handle.Push();
                        _userDataHandleCache[(obj, descriptor)] = handle;
                        return;
                    }
                }
                else
                {
                    if (UserDataDescriptorProvider.TryGetDescriptor<T>(obj, out var descriptor))
                    {
                        new UserDataHandle<T>(Lua, obj, descriptor).Push();
                        return;
                    }
                }

                switch (obj)
                {
                    case Delegate:
                    {
                        if (obj is LuaCFunction)
                        {
                            lua_pushcfunction(L, (LuaCFunction) (object) obj);
                        }
                        else
                        {
                            Throw.ArgumentException("The delegate cannot be marshaled without a user data descriptor.", nameof(obj));
                        }

                        return;
                    }
                    case IEnumerable:
                    {
                        PushEnumerable((IEnumerable) obj);
                        return;
                    }
                    case IConvertible:
                    {
                        switch (((IConvertible) obj).GetTypeCode())
                        {
                            case TypeCode.Boolean:
                            {
                                lua_pushboolean(L, ((IConvertible) obj).ToBoolean(Lua.Culture));
                                return;
                            }
                            case TypeCode.SByte:
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            {
                                lua_pushinteger(L, ((IConvertible) obj).ToInt64(Lua.Culture));
                                return;
                            }
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                            {
                                if (typeof(lua_Number) == typeof(double))
                                {
                                    lua_pushnumber(L, ((IConvertible) obj).ToDouble(Lua.Culture));
                                }
                                else
                                {
                                    lua_pushnumber(L, (lua_Number) ((IConvertible) obj).ToType(typeof(lua_Number), Lua.Culture));
                                }

                                return;
                            }
                            case TypeCode.Char:
                            {
                                var charValue = ((IConvertible) obj).ToChar(Lua.Culture);
#if NET7_0_OR_GREATER
                                lua_pushstring(L, new ReadOnlySpan<char>(in charValue));
#else
                                lua_pushstring(L, MemoryMarshal.CreateReadOnlySpan(ref charValue, 1));
#endif
                                return;
                            }
                            case TypeCode.String:
                            {
                                lua_pushstring(L, ((IConvertible) obj).ToString(Lua.Culture));
                                return;
                            }
                            default:
                            {
                                throw new ArgumentOutOfRangeException(nameof(obj), $"The convertible object type '{((IConvertible) obj).GetTypeCode()}' cannot be marshaled.");
                            }
                        }
                    }
                    default:
                    {
                        Throw.ArgumentException($"The object type {obj.GetType()} cannot be marshaled.", nameof(obj));
                        return;
                    }
                }
            }
        }
    }
}
