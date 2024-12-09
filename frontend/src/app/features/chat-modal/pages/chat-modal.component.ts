import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FileUploadDto } from '../../documents/data/file';

@Component({
  selector: 'app-chat-modal',
  imports: [CommonModule, FormsModule],
  templateUrl: './chat-modal.component.html',
  styleUrl: './chat-modal.component.css'
})
export class ChatModalComponent {
  @Input() isVisible = false; // Controla la visibilidad del modal
  @Input() document?: FileUploadDto;
  @Output() closeModalEvent = new EventEmitter<void>(); // Evento para cerrar el modal

  // Lista de mensajes del chat
  messages: { text: string; sender: 'system' | 'user' | 'bot' }[] = [];

  // Mensaje que se escribe en el input
  newMessage: string = '';

  ngOnChanges(changes: SimpleChanges) {
    if (changes['document'] && this.document) {
      this.resetChat();
    }
  }

  // Método para cerrar el modal
  closeModal() {
    this.resetChat();
    this.closeModalEvent.emit();
  }

  // Método para enviar un mensaje
  sendMessage() {
    if (this.newMessage.trim()) {
      this.messages.push({ text: this.newMessage, sender: 'user' });

      this.newMessage = '';

      setTimeout(() => {
        this.messages.push({ text: 'Respuesta del bot', sender: 'bot' });
      }, 500);
    }
  }

  resetChat() {
    this.messages = [
      { text: 'Hazme una pregunta sobre '+this.document?.name, sender: 'system' }
    ];
    this.newMessage = '';
  }
}
