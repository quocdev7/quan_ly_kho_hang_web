import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private ma: string = '';

  setMa(value: string) {
    this.ma = value;
  }

  getMa(): string {
    return this.ma;
  }
}
