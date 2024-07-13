import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  http = inject(HttpClient)
  title = 'DatingApp';
  users: any;
  ngOnInit(): void {
    this.http.get('http://localhost:5045/api/users').subscribe({
      next: res => {
        this.users = res;
        console.log(this.users);
      },
      error: e => {
        console.log(e);
      },
      complete: () =>{
        console.log('Acabou');
      }
    })
  }
}
