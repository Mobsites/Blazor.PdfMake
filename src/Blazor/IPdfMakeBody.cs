// Copyright (c) 2020 Allan Mobley. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Mobsites.Blazor
{
    public interface IPdfMakeBody
    {
        Task<object> GetDocumentDefinition();
    }
}