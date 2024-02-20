import { Component } from '@angular/core';

@Component({
  selector: 'app-loader',
  template: `
            <div class="loader_wrapper">
              <div class="loader_box">
                <span class="loader"></span>
              </div>
            </div>
  `,
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent {

}
