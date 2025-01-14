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
  @Input() isVisible = false;
  @Input() document?: FileUploadDto;
  @Output() closeModalEvent = new EventEmitter<void>();
  @ViewChild('messagesContainer') messagesContainer!: ElementRef;

  messages: { text: string; sender: 'system' | 'user' | 'bot' }[] = [];

  newMessage: string = '';
  isLoading = false;
  currentBotMessage: string = '';

  constructor(private chatService: ChatService) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes['document'] && this.document) {
      this.resetChat();
    }
  }

  closeModal() {
    this.resetChat();
    this.closeModalEvent.emit();
  }

  sendMessage() {
    if (this.newMessage.trim() && this.document?.id) {
      const userMessage = this.newMessage.trim();
      this.messages.push({ text: userMessage, sender: 'user' });
      this.newMessage = '';
      this.scrollToBottom();
      
      this.isLoading = true;
      const botMessage = { text: '', sender: 'bot' as const };
      this.messages.push(botMessage);

      this.chatService.askQuestion(userMessage, this.document.id).subscribe({
        next: (chunk: string) => {
          if (chunk && chunk.trim()) {
            botMessage.text = botMessage.text + chunk;
            this.scrollToBottom();
          }
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
      }, 0); 
    }
  }
}
