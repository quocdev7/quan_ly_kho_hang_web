import { Component, ElementRef, EventEmitter, HostBinding, HostListener, Input, OnChanges, OnDestroy, OnInit, Output, Renderer2, SimpleChanges, ViewEncapsulation } from '@angular/core';
import { animate, AnimationBuilder, AnimationPlayer, style } from '@angular/animations';
import { FuseTreeMode, FuseTreePosition } from '@fuse/components/tree/tree.types';
import { FuseUtilsService } from '@fuse/services/utils/utils.service';
import { BooleanInput, coerceBooleanProperty } from '@angular/cdk/coercion';
import { FuseTreeService } from './tree.service';

@Component({
    selector     : 'fuse-tree',
    templateUrl  : './tree.component.html',
    styleUrls    : ['./tree.component.scss'],
    encapsulation: ViewEncapsulation.None,
    exportAs     : 'FuseTree'
})
export class FuseTreeComponent implements OnChanges, OnInit, OnDestroy
{
    /* eslint-disable @typescript-eslint/naming-convention */
    static ngAcceptInputType_fixed: BooleanInput;
    static ngAcceptInputType_opened: BooleanInput;
    static ngAcceptInputType_transparentOverlay: BooleanInput;
    /* eslint-enable @typescript-eslint/naming-convention */

    @Input() fixed: boolean = false;
    @Input() mode: FuseTreeMode = 'side';
    @Input() name: string = this._fuseUtilsService.randomId();
    @Input() opened: boolean = false;
    @Input() position: FuseTreePosition = 'left';
    @Input() transparentOverlay: boolean = false;
    @Output() readonly fixedChanged: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() readonly modeChanged: EventEmitter<FuseTreeMode> = new EventEmitter<FuseTreeMode>();
    @Output() readonly openedChanged: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() readonly positionChanged: EventEmitter<FuseTreePosition> = new EventEmitter<FuseTreePosition>();

    private _animationsEnabled: boolean = false;
    private _hovered: boolean = false;
    private _overlay: HTMLElement;
    private _player: AnimationPlayer;

    /**
     * Constructor
     */
    constructor(
        private _animationBuilder: AnimationBuilder,
        private _elementRef: ElementRef,
        private _renderer2: Renderer2,
        private _FuseTreeService: FuseTreeService,
        private _fuseUtilsService: FuseUtilsService
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Host binding for component classes
     */
    @HostBinding('class') get classList(): any
    {
        return {
            'fuse-tree-animations-enabled'         : this._animationsEnabled,
            'fuse-tree-fixed'                      : this.fixed,
            'fuse-tree-hover'                      : this._hovered,
            [`fuse-tree-mode-${this.mode}`]        : true,
            'fuse-tree-opened'                     : this.opened,
            [`fuse-tree-position-${this.position}`]: true
        };
    }

    /**
     * Host binding for component inline styles
     */
    @HostBinding('style') get styleList(): any
    {
        return {
            'visibility': this.opened ? 'visible' : 'hidden'
        };
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Decorated methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * On mouseenter
     *
     * @private
     */
    @HostListener('mouseenter')
    private _onMouseenter(): void
    {
        // Enable the animations
        this._enableAnimations();

        // Set the hovered
        this._hovered = true;
    }

    /**
     * On mouseleave
     *
     * @private
     */
    @HostListener('mouseleave')
    private _onMouseleave(): void
    {
        // Enable the animations
        this._enableAnimations();

        // Set the hovered
        this._hovered = false;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On changes
     *
     * @param changes
     */
    ngOnChanges(changes: SimpleChanges): void
    {
        // Fixed
        if ( 'fixed' in changes )
        {
            // Coerce the value to a boolean
            this.fixed = coerceBooleanProperty(changes.fixed.currentValue);

            // Execute the observable
            this.fixedChanged.next(this.fixed);
        }

        // Mode
        if ( 'mode' in changes )
        {
            // Get the previous and current values
            const previousMode = changes.mode.previousValue;
            const currentMode = changes.mode.currentValue;

            // Disable the animations
            this._disableAnimations();

            // If the mode changes: 'over -> side'
            if ( previousMode === 'over' && currentMode === 'side' )
            {
                // Hide the overlay
                this._hideOverlay();
            }

            // If the mode changes: 'side -> over'
            if ( previousMode === 'side' && currentMode === 'over' )
            {
                // If the tree is opened
                if ( this.opened )
                {
                    // Show the overlay
                    this._showOverlay();
                }
            }

            // Execute the observable
            this.modeChanged.next(currentMode);

            // Enable the animations after a delay
            // The delay must be bigger than the current transition-duration
            // to make sure nothing will be animated while the mode is changing
            setTimeout(() => {
                this._enableAnimations();
            }, 500);
        }

        // Opened
        if ( 'opened' in changes )
        {
            // Coerce the value to a boolean
            const open = coerceBooleanProperty(changes.opened.currentValue);

            // Open/close the tree
            this._toggleOpened(open);
        }

        // Position
        if ( 'position' in changes )
        {
            // Execute the observable
            this.positionChanged.next(this.position);
        }

        // Transparent overlay
        if ( 'transparentOverlay' in changes )
        {
            // Coerce the value to a boolean
            this.transparentOverlay = coerceBooleanProperty(changes.transparentOverlay.currentValue);
        }
    }

    /**
     * On init
     */
    ngOnInit(): void
    {
        // Register the tree
        this._FuseTreeService.registerComponent(this.name, this);
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Deregister the tree from the registry
        this._FuseTreeService.deregisterComponent(this.name);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Open the tree
     */
    open(): void
    {
        // Return if the tree has already opened
        if ( this.opened )
        {
            return;
        }

        // Open the tree
        this._toggleOpened(true);
    }

    /**
     * Close the tree
     */
    close(): void
    {
        // Return if the tree has already closed
        if ( !this.opened )
        {
            return;
        }

        // Close the tree
        this._toggleOpened(false);
    }

    /**
     * Toggle the tree
     */
    toggle(): void
    {
        if ( this.opened )
        {
            this.close();
        }
        else
        {
            this.open();
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Enable the animations
     *
     * @private
     */
    private _enableAnimations(): void
    {
        // Return if the animations are already enabled
        if ( this._animationsEnabled )
        {
            return;
        }

        // Enable the animations
        this._animationsEnabled = true;
    }

    /**
     * Disable the animations
     *
     * @private
     */
    private _disableAnimations(): void
    {
        // Return if the animations are already disabled
        if ( !this._animationsEnabled )
        {
            return;
        }

        // Disable the animations
        this._animationsEnabled = false;
    }

    /**
     * Show the backdrop
     *
     * @private
     */
    private _showOverlay(): void
    {
        // Create the backdrop element
        this._overlay = this._renderer2.createElement('div');

        // Return if overlay couldn't be create for some reason
        if ( !this._overlay )
        {
            return;
        }

        // Add a class to the backdrop element
        this._overlay.classList.add('fuse-tree-overlay');

        // Add a class depending on the fixed option
        if ( this.fixed )
        {
            this._overlay.classList.add('fuse-tree-overlay-fixed');
        }

        // Add a class depending on the transparentOverlay option
        if ( this.transparentOverlay )
        {
            this._overlay.classList.add('fuse-tree-overlay-transparent');
        }

        // Append the backdrop to the parent of the tree
        this._renderer2.appendChild(this._elementRef.nativeElement.parentElement, this._overlay);

        // Create the enter animation and attach it to the player
        this._player = this._animationBuilder.build([
            animate('300ms cubic-bezier(0.25, 0.8, 0.25, 1)', style({opacity: 1}))
        ]).create(this._overlay);

        // Play the animation
        this._player.play();

        // Add an event listener to the overlay
        this._overlay.addEventListener('click', () => {
            this.close();
        });
    }

    /**
     * Hide the backdrop
     *
     * @private
     */
    private _hideOverlay(): void
    {
        if ( !this._overlay )
        {
            return;
        }

        // Create the leave animation and attach it to the player
        this._player = this._animationBuilder.build([
            animate('300ms cubic-bezier(0.25, 0.8, 0.25, 1)', style({opacity: 0}))
        ]).create(this._overlay);

        // Play the animation
        this._player.play();

        // Once the animation is done...
        this._player.onDone(() => {

            // If the backdrop still exists...
            if ( this._overlay )
            {
                // Remove the backdrop
                this._overlay.parentNode.removeChild(this._overlay);
                this._overlay = null;
            }
        });
    }

    /**
     * Open/close the tree
     *
     * @param open
     * @private
     */
    private _toggleOpened(open: boolean): void
    {
        // Set the opened
        this.opened = open;

        // Enable the animations
        this._enableAnimations();

        // If the mode is 'over'
        if ( this.mode === 'over' )
        {
            // If the tree opens, show the overlay
            if ( open )
            {
                this._showOverlay();
            }
            // Otherwise, close the overlay
            else
            {
                this._hideOverlay();
            }
        }

        // Execute the observable
        this.openedChanged.next(open);
    }
}
