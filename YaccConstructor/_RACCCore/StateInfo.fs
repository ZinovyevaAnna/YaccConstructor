﻿// StateInfo.fs
//
// Copyright 2009-2010 Semen Grigorev
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation.

namespace Yard.Generators._RACCGenerator

type StateInfo<'position, 'tree, 'itemName, 'traceStep> =
    {
        position : 'position
        forest   : List<'tree>
        itemName : 'itemName    
        sTrace    : List<'traceStep>
    }
