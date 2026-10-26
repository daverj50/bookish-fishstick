import { Component, inject, OnInit, signal } from "@angular/core";
import { Header } from "./layout/header/header";
import { HttpClient } from "@angular/common/http";
import { Product } from "./shared/models/product";
import { Pagination } from "./shared/models/pagination";

@Component({
  imports: [Header],
  selector: "app-root",
  styleUrl: "./app.css",
  templateUrl: "./app.html",
})
export class App implements OnInit {
  private http = inject(HttpClient);
  protected readonly baseUrl = "https://localhost:5001/api/";
  protected readonly title = signal("Skinet");
  protected readonly products = signal<Product[]>([]);

  ngOnInit(): void {
    this.http.get<Pagination<Product>>(this.baseUrl + "products").subscribe({
      next: (data) => this.products.set(data.data),
      error: (err) => console.log(err.message),
      complete: () => console.log("Done"),
    });
  }
}
