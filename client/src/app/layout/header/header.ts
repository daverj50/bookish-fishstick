import { Component } from "@angular/core";
import { MatBadge } from "@angular/material/badge";
import { MatButton } from "@angular/material/button";
import { MatIcon } from "@angular/material/icon";

@Component({
  imports: [MatBadge, MatIcon, MatButton],
  selector: "app-header",
  styleUrl: "./header.css",
  templateUrl: "./header.html",
})
export class Header {}
