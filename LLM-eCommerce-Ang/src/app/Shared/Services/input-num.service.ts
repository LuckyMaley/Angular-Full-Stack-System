import { Injectable, Renderer2 } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class InputNumService {

  constructor() { }

  validateNumberInput(input: HTMLInputElement) {
    const value = input.value;
    const regex = /^\d*\.?\d*$/;
    if (!regex.test(value)) {
      input.value = value.slice(0, -1);
    }
  }
}
