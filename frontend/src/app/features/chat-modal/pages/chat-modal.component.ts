import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FileUploadDto } from '../../documents/data/file';
import { ChatService } from '../data/chat.service';

@Component({
  selector: 'app-chat-modal',
  imports: [CommonModule, FormsModule],
  templateUrl: './chat-modal.component.html',
  styleUrl: './chat-modal.component.css',
})
export class ChatModalComponent {
  @Input() isVisible = false; // Controla la visibilidad del modal
  @Input() document?: FileUploadDto;
  @Output() closeModalEvent = new EventEmitter<void>(); // Evento para cerrar el modal
  @ViewChild('messagesContainer') messagesContainer!: ElementRef;

  // Lista de mensajes del chat
  messages: { text: string; sender: 'system' | 'user' | 'bot' }[] = [];

  // Mensaje que se escribe en el input
  newMessage: string = '';
  isLoading = false;
  currentBotMessage: string = '';

  constructor(private chatService: ChatService) {}

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
    if (this.newMessage.trim() && this.document?.id) {
      const userMessage = this.newMessage.trim();
      this.messages.push({ text: userMessage, sender: 'user' });
      this.newMessage = '';
      this.scrollToBottom();
      
      this.isLoading = true;
      this.currentBotMessage = ''; // Reset mensaje actual del bot

      // Crear nuevo mensaje del bot vacío
      this.messages.push({ text: '', sender: 'bot' });
      const botMessageIndex = this.messages.length - 1;

      this.chatService.askQuestion(userMessage, this.document.id).subscribe({
        next: (chunk) => {
          // Acumular el chunk en el mensaje actual
          this.currentBotMessage += chunk;
          // Actualizar el último mensaje del bot
          this.messages[botMessageIndex].text = this.currentBotMessage;
          this.scrollToBottom();
        },
        error: (error) => {
          console.error('Error:', error);
          this.messages.push({
            text: 'Lo siento, ha ocurrido un error al procesar tu pregunta.',
            sender: 'system'
          });
          this.scrollToBottom();
        },
        complete: () => {
          this.isLoading = false;
          this.currentBotMessage = ''; // Limpiar el mensaje actual
          this.scrollToBottom();
        }
      });
    }
  }

  resetChat() {
    this.messages = [
      {
        text: `¿Qué te gustaría saber sobre ${this.document?.name}?`,
        sender: 'system',
      },
    ];
    this.newMessage = '';
    this.isLoading = false;
    setTimeout(() => this.scrollToBottom(), 0);
  }

  private scrollToBottom() {
    if (this.messagesContainer) {
      setTimeout(() => {
        const nativeElement = this.messagesContainer.nativeElement;
        nativeElement.scrollTop = nativeElement.scrollHeight;
      }, 0); // Retraso ligero para esperar la actualización del DOM
    }
  }
}
