﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Laylua.Moon;
using Laylua.Moon.Enums;
using Laylua.Moon.Native.Structures;

namespace Laylua.Library.Entities.Reference.Thread;

/// <summary>
///     Represents a reference to a Lua thread.
/// </summary>
/// <remarks>
///     <inheritdoc cref="LuaReference"/>
/// </remarks>
public sealed unsafe class LuaThread : LuaReference
{
    /// <summary>
    ///     Gets the status of this thread.
    /// </summary>
    public LuaStatus Status
    {
        get
        {
            ThrowIfInvalid();

            return lua_status(_l);
        }
    }

    internal lua_State* State
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _l;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            Debug.Assert(_l == default);
            _l = value;
        }
    }

    private lua_State* _l;

    internal LuaThread()
    { }

    internal static LuaThread CreateMainThread(Lua lua)
    {
        var thread = new LuaThread();
        thread.Lua = lua;
        thread.Reference = LuaRegistry.Indices.MainThread;
        thread._l = lua.GetStatePointer();

#pragma warning disable CA1816
        GC.SuppressFinalize(thread);
#pragma warning restore CA1816

        return thread;
    }

    internal override void ResetFields()
    {
        _l = default;
    }

    /// <inheritdoc cref="LuaReference.Clone{T}"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public LuaThread Clone()
    {
        return Clone<LuaThread>();
    }
}
