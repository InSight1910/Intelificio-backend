import { CommonModule } from '@angular/common';
import {
  Component,
  ContentChild,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { isEarlyEventType } from '@angular/core/primitives/event-dispatch';
import { FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-modal-package',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.css',
})
export class ModalPackageComponent {
  @ContentChild(FormGroupDirective) formDirective!: FormGroupDirective;
  @Output() onModalSubmit = new EventEmitter<void>();
  @Output() close = new EventEmitter<void>();

  @Input() title: string = '';
  @Input() isValid: boolean = false;
  @Input() buttonTitle: string = '';
  @Input() isLoading: boolean = false;
  @Input() showActionButton: boolean = true;
  @Input() isDeleting: boolean = false;

  onSubmit(event: Event) {
    if (!this.isLoading || this.isValid) {
      this.formDirective.onSubmit(event);
      this.onModalSubmit.emit();
    }
  }

  onClose() {
    this.close.emit();
  }
}
