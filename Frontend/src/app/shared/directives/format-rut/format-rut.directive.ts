import { Directive, ElementRef, Host, HostListener } from '@angular/core';

@Directive({
  selector: '[appFormatRut]',
  standalone: true,
})
export class FormatRutDirective {
  constructor(private el: ElementRef) {}

  @HostListener('input', ['$event']) onInputChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const value = input.value.replace(/[^0-9kK]/g, '');

    const body = value.slice(0, -1);
    const dv = value.slice(-1);

    let formattedBody = '';
    for (let i = body.length - 1, count = 0; i >= 0; i--, count++) {
      formattedBody = body[i] + formattedBody;
      if (i > 0 && count % 3 === 2) {
        formattedBody = `.${formattedBody}`;
      }
    }
    if (input.value.length > 0) {
      input.value = `${formattedBody}-${dv}`;
    } else {
      input.value = '';
    }
  }
}
