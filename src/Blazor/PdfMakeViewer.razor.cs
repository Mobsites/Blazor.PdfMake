// Copyright (c) 2020 Allan Mobley. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mobsites.Blazor
{
    /// <summary>
    /// UI component for rendering a pdf.
    /// </summary>
    public partial class PdfMakeViewer
    {
        /****************************************************
        *
        *  PUBLIC INTERFACE
        *
        ****************************************************/

        /// <summary>
        /// Content to render.
        /// </summary>
        [JSInvokable]
        public void SetIndex(int index)
        {
            if (Index < 0)
            {
                Index = index;
            }
        }

        /// <summary>
        /// Clear all state for this UI component and any of its dependents from browser storage.
        /// </summary>
        public ValueTask ClearState() => this.ClearState<PdfMakeViewer, Options>();



        /****************************************************
        *
        *  NON-PUBLIC INTERFACE
        *
        ****************************************************/

        /// <summary>
        /// Whether component environment is Blazor WASM or Server.
        /// </summary>
        internal bool IsWASM => RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY"));

        private DotNetObjectReference<PdfMakeViewer> self;

        /// <summary>
        /// Net reference passed into javascript representation.
        /// </summary>
        protected DotNetObjectReference<PdfMakeViewer> Self
        {
            get => self ?? (Self = DotNetObjectReference.Create(this));
            set => self = value;
        }

        /// <summary>
        /// The index to this object's javascript representation in the object store.
        /// </summary>
        internal int Index { get; set; } = -1;

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference Container { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference Title { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference PageNumber { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference Previous { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference Next { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference ZoomOut { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference ZoomIn { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference ErrorWrapper { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference ErrorMessage { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference ErrorClose { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference ErrorMoreInfo { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference ErrorShowMore { get; set; }

        /// <summary>
        /// Dom element reference passed into javascript representation.
        /// </summary>
        internal ElementReference ErrorShowLess { get; set; }
        
        /// <summary>
        /// Life cycle method for when component has been rendered in the dom and javascript interopt is fully ready.
        /// </summary>
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Initialize();
            }
            else
            {
                await Update();
            }
        }

        /// <summary>
        /// Initialize state and javascript representations.
        /// </summary>
        private async Task Initialize()
        {
            var options = await this.GetState<PdfMakeViewer, Options>();

            if (options is null)
            {
                options = this.GetOptions();
            }
            else
            {
                await this.CheckState(options);
            }

            this.initialized = await this.jsRuntime.InvokeAsync<bool>(
                "Mobsites.Blazor.PdfMake.init",
                Self,
                new
                {
                    this.Container,
                    this.Title,
                    this.PageNumber,
                    this.Previous,
                    this.Next,
                    this.ZoomOut,
                    this.ZoomIn,
                    this.ErrorWrapper,
                    this.ErrorMessage,
                    this.ErrorClose,
                    this.ErrorMoreInfo,
                    this.ErrorShowMore,
                    this.ErrorShowLess
                },
                options,
                false);

            await this.Save<PdfMakeViewer, Options>(options);
        }

        /// <summary>
        /// Update state.
        /// </summary>
        private async Task Update()
        {
            var options = await this.GetState<PdfMakeViewer, Options>();

            // Use current state if...
            if (this.initialized || options is null)
            {
                options = this.GetOptions();
            }

            await this.jsRuntime.InvokeVoidAsync(
                "Mobsites.Blazor.PdfMake.update",
                Index,
                options);

            await this.Save<PdfMakeViewer, Options>(options);
        }

        /// <summary>
        /// Get current or storage-saved options for keeping state.
        /// </summary>
        protected Options GetOptions()
        {
            var options = new Options 
            {
                
            };

            base.SetOptions(options);

            return options;
        }

        /// <summary>
        /// Check whether storage-retrieved options are different than current
        /// and thereby need to notify parents of change when keeping state.
        /// </summary>
        protected async Task CheckState(Options options)
        {
            bool stateChanged = false;
            

            if (await base.CheckState(options) || stateChanged)
                StateHasChanged();
        }

        /// <summary>
        /// Called by GC.
        /// </summary>
        public override void Dispose()
        {
            jsRuntime.InvokeVoidAsync("Mobsites.Blazor.PdfMake.destroy", Index);
            self?.Dispose();
            base.Dispose();
        }
    }
}