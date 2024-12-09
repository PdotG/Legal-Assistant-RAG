import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

interface ErrorData {
  code: number;
  message: string;
  title: string;
}

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent implements OnInit {
  errorData: ErrorData = {
    code: 404,
    title: 'Page Not Found',
    message: 'The page you are looking for does not exist.'
  };

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      if (data['error']) {
        this.errorData = data['error'];
      }
    });
  }
}
