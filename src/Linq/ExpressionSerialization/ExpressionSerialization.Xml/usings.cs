﻿global using System.Collections;
global using System.Collections.Concurrent;
global using System.Collections.Frozen;
global using System.Collections.Immutable;
global using System.Collections.ObjectModel;
global using System.Diagnostics;
global using System.Linq.Expressions;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Runtime.Serialization;
global using System.Text;
global using System.Xml;
global using System.Xml.Linq;
global using System.Xml.Schema;

global using vm2.ExpressionSerialization.Shared.Abstractions;
global using vm2.ExpressionSerialization.Shared.Conventions;
global using vm2.ExpressionSerialization.Shared.Exceptions;
global using vm2.ExpressionSerialization.Shared.Extensions;
global using vm2.Threading.ReadersWriter;

global using static vm2.ExpressionSerialization.Shared.Extensions.DebugExtensions;