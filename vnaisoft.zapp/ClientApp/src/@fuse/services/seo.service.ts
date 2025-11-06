import { Injectable } from '@angular/core';
import { Title, Meta, MetaDefinition } from '@angular/platform-browser';
@Injectable({
  providedIn: 'root'
})
export class SeoService {
  constructor(public title: Title, public meta: Meta) { }

  public updateTitle(title: string){
    this.title.setTitle(title);
  }
  public updateMetaTags(metaTags: MetaDefinition[]){
    metaTags.forEach(m=> this.meta.updateTag(m));
  }
}