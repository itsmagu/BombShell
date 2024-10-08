﻿using System.Runtime.CompilerServices;
using Laylua.Moon.Enums;
using Laylua.Moon.Native.Structures;

namespace Laylua.Library.Entities.Reference.Function;

/// <summary>
///     Represents a reference to a Lua function.
/// </summary>
/// <remarks>
///     <inheritdoc cref="LuaReference"/>
/// </remarks>
public sealed unsafe partial class LuaFunction : LuaReference
{
    /// <summary>
    ///     Gets the upvalues of this function.
    /// </summary>
    public UpvalueCollection Upvalues => new(this);

    internal LuaFunction()
    { }

    /// <inheritdoc cref="LuaReference.Clone{T}"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public LuaFunction Clone()
    {
        return Clone<LuaFunction>();
    }

    private static void PrepareFunction(LuaReference reference, out int top, int argumentCount)
    {
        reference.Lua.Stack.EnsureFreeCapacity(argumentCount + 1);

        lua_State* L = reference.Lua.GetStatePointer();
        top = lua_gettop(L);

        PushValue(reference);
    }

    internal static LuaFunctionResults PCall(Lua lua, int oldTop, int argumentCount)
    {
        lua_State* L = lua.GetStatePointer();
        LuaStatus status = lua_pcall(L, argumentCount, LUA_MULTRET, 0);
        if (status.IsError())
        {
            lua.ThrowLuaException(status);
        }

        int newTop = lua_gettop(L);
        LuaStackValueRange range = LuaStackValueRange.FromTop(lua.Stack, oldTop, newTop);
        return new LuaFunctionResults(range);
    }

    /// <summary>
    ///     Calls (invokes) this function without any arguments.
    /// </summary>
    /// <returns>
    ///     The results returned from the function.
    /// </returns>
    public LuaFunctionResults Call()
    {
        ThrowIfInvalid();

        PrepareFunction(this, out int top, 0);

        return PCall(Lua, top, 0);
    }

    /// <summary>
    ///     Calls (invokes) this function with the specified argument.
    /// </summary>
    /// <typeparam name="T"> The type of the argument. </typeparam>
    /// <param name="argument"> The argument to pass to the function. </param>
    /// <returns>
    ///     The results returned from the function.
    /// </returns>
    public LuaFunctionResults Call<T>(T? argument)
    {
        ThrowIfInvalid();

        PrepareFunction(this, out int top, 1);

        try
        {
            Lua.Marshaler.PushValue(argument);

            return PCall(Lua, top, 1);
        }
        catch
        {
            lua_settop(Lua.GetStatePointer(), top);
            throw;
        }
    }

    /// <summary>
    ///     Calls (invokes) this function with the specified arguments.
    /// </summary>
    /// <typeparam name="T1"> The type of the first argument. </typeparam>
    /// <typeparam name="T2"> The type of the second argument. </typeparam>
    /// <param name="argument1"> The first argument to pass to the function. </param>
    /// <param name="argument2"> The second argument to pass to the function. </param>
    /// <returns>
    ///     The results returned from the function.
    /// </returns>
    public LuaFunctionResults Call<T1, T2>(T1? argument1, T2? argument2)
    {
        ThrowIfInvalid();

        PrepareFunction(this, out int top, 2);

        try
        {
            Lua.Marshaler.PushValue(argument1);
            Lua.Marshaler.PushValue(argument2);

            return PCall(Lua, top, 2);
        }
        catch
        {
            lua_settop(Lua.GetStatePointer(), top);
            throw;
        }
    }

    /// <summary>
    ///     Calls (invokes) this function with the specified arguments.
    /// </summary>
    /// <typeparam name="T1"> The type of the first argument. </typeparam>
    /// <typeparam name="T2"> The type of the second argument. </typeparam>
    /// <typeparam name="T3"> The type of the third argument. </typeparam>
    /// <param name="argument1"> The first argument to pass to the function. </param>
    /// <param name="argument2"> The second argument to pass to the function. </param>
    /// <param name="argument3"> The third argument to pass to the function. </param>
    /// <returns>
    ///     The results returned from the function.
    /// </returns>
    public LuaFunctionResults Call<T1, T2, T3>(T1? argument1, T2? argument2, T3? argument3)
    {
        ThrowIfInvalid();

        PrepareFunction(this, out int top, 3);

        try
        {
            Lua.Marshaler.PushValue(argument1);
            Lua.Marshaler.PushValue(argument2);
            Lua.Marshaler.PushValue(argument3);

            return PCall(Lua, top, 3);
        }
        catch
        {
            lua_settop(Lua.GetStatePointer(), top);
            throw;
        }
    }

    /// <summary>
    ///     Calls (invokes) this function with the specified arguments.
    /// </summary>
    /// <typeparam name="T1"> The type of the first argument. </typeparam>
    /// <typeparam name="T2"> The type of the second argument. </typeparam>
    /// <typeparam name="T3"> The type of the third argument. </typeparam>
    /// <typeparam name="T4"> The type of the fourth argument. </typeparam>
    /// <param name="argument1"> The first argument to pass to the function. </param>
    /// <param name="argument2"> The second argument to pass to the function. </param>
    /// <param name="argument3"> The third argument to pass to the function. </param>
    /// <param name="argument4"> The fourth argument to pass to the function. </param>
    /// <returns>
    ///     The results returned from the function.
    /// </returns>
    public LuaFunctionResults Call<T1, T2, T3, T4>(T1? argument1, T2? argument2, T3? argument3, T4? argument4)
    {
        ThrowIfInvalid();

        PrepareFunction(this, out int top, 4);

        try
        {
            Lua.Marshaler.PushValue(argument1);
            Lua.Marshaler.PushValue(argument2);
            Lua.Marshaler.PushValue(argument3);
            Lua.Marshaler.PushValue(argument4);

            return PCall(Lua, top, 4);
        }
        catch
        {
            lua_settop(Lua.GetStatePointer(), top);
            throw;
        }
    }

    /// <summary>
    ///     Calls (invokes) this function with the specified arguments.
    /// </summary>
    /// <param name="arguments"> The arguments to pass to the function. </param>
    /// <returns>
    ///     The results returned from the function.
    /// </returns>
    public LuaFunctionResults Call(params object?[] arguments)
    {
        ThrowIfInvalid();

        int argumentCount = arguments.Length;

        PrepareFunction(this, out int top, argumentCount);

        try
        {
            foreach (object? argument in arguments)
            {
                Lua.Marshaler.PushValue(argument);
            }

            return PCall(Lua, top, argumentCount);
        }
        catch
        {
            lua_settop(Lua.GetStatePointer(), top);
            throw;
        }
    }

    /// <summary>
    ///     Calls (invokes) this function with the specified arguments.
    /// </summary>
    /// <param name="arguments"> The arguments to pass to the function. </param>
    /// <returns>
    ///     The results returned from the function.
    /// </returns>
    public LuaFunctionResults Call(params LuaStackValue[] arguments)
    {
        ThrowIfInvalid();

        int argumentCount = arguments.Length;

        PrepareFunction(this, out int top, argumentCount);

        try
        {
            foreach (LuaStackValue argument in arguments)
            {
                argument.PushValue();
            }

            return PCall(Lua, top, argumentCount);
        }
        catch
        {
            lua_settop(Lua.GetStatePointer(), top);
            throw;
        }
    }

    /// <summary>
    ///     Calls (invokes) this function with the specified arguments.
    /// </summary>
    /// <param name="arguments"> The arguments to pass to the function. </param>
    /// <returns>
    ///     The results returned from the function.
    /// </returns>
    public LuaFunctionResults Call(LuaStackValueRange arguments)
    {
        ThrowIfInvalid();

        int argumentCount = arguments.Count;

        PrepareFunction(this, out int top, argumentCount);

        try
        {
            foreach (LuaStackValue argument in arguments)
            {
                argument.PushValue();
            }

            return PCall(Lua, top, argumentCount);
        }
        catch
        {
            lua_settop(Lua.GetStatePointer(), top);
            throw;
        }
    }
}
