import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appFormatPhone]',
  standalone: true,
})
export class FormatPhoneDirective {
  constructor(private el: ElementRef) {}

  @HostListener('input', ['$event']) onInputChange(event: any) {
    const input = this.el.nativeElement;

    // Remove all non-numeric characters
    let cleaned = input.value.replace(/\D/g, '');

    // Format the number if it's 9 digits long
    if (cleaned.length === 9) {
      input.value = `9 ${cleaned.slice(1, 5)} ${cleaned.slice(5)}`;
    }
  }
}
