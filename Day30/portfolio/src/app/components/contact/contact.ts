
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './contact.html',
  styleUrl: './contact.css'
})
export class Contact {

  name: string = '';
  email: string = '';
  message: string = '';

  // Replace this with your actual email address.
  contactEmail: string = 'devadharshinis1308@gmail.com';

  onSubmit(event: Event): void {
    event.preventDefault();

    const subject = encodeURIComponent(
      `Portfolio contact from ${this.name}`
    );

    const body = encodeURIComponent(
      `Name: ${this.name}\n` +
      `Email: ${this.email}\n\n` +
      `Message:\n${this.message}`
    );

    const mailtoUrl =
      `mailto:${this.contactEmail}?subject=${subject}&body=${body}`;

      
    window.location.href = mailtoUrl;

    alert('Form is Submitted Successfully');
  }
}
