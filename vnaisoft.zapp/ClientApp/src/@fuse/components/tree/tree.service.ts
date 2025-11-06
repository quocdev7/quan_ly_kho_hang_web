import { Injectable } from '@angular/core';
import { FuseTreeComponent } from '@fuse/components/tree/tree.component';

@Injectable({
    providedIn: 'root'
})
export class FuseTreeService
{
    private _componentRegistry: Map<string, FuseTreeComponent> = new Map<string, FuseTreeComponent>();

    /**
     * Constructor
     */
    constructor()
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Register tree component
     *
     * @param name
     * @param component
     */
    registerComponent(name: string, component: FuseTreeComponent): void
    {
        this._componentRegistry.set(name, component);
    }

    /**
     * Deregister tree component
     *
     * @param name
     */
    deregisterComponent(name: string): void
    {
        this._componentRegistry.delete(name);
    }

    /**
     * Get tree component from the registry
     *
     * @param name
     */
    getComponent(name: string): FuseTreeComponent | undefined
    {
        return this._componentRegistry.get(name);
    }
}
