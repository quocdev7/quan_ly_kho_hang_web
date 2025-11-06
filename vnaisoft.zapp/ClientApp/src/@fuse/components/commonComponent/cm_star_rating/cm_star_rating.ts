import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
    selector: 'cm_star_rating',
    templateUrl: './cm_star_rating.html',
    styleUrls: ['./cm_star_rating.scss'],
    encapsulation: ViewEncapsulation.Emulated
})
export class cm_star_rating implements OnInit {
    @Input('disabled') public disabled: boolean = true;
    @Input('rating') public rating: number = 3;
    @Input('starCount') public starCount: number = 5;
    @Input('color') public color: string = 'accent';
    @Output() public ratingUpdated = new EventEmitter();

    private snackBarDuration: number = 2000;
    public ratingArr = [];

    constructor(private snackBar: MatSnackBar) {
    }


    ngOnInit() {
        console.log("a " + this.starCount)
        for (let index = 0; index < this.starCount; index++) {
            this.ratingArr.push(index);
        }
    }
    onClick(rating: number) {
        console.log(rating)
        this.snackBar.open('You rated ' + rating + ' / ' + this.starCount, '', {
            duration: this.snackBarDuration
        });
        this.ratingUpdated.emit(rating);
        return false;
    }

    showIcon(index: number) {
        if (this.rating >= index + 1) {
            return 'star';
        } else {
            return 'star_border';
        }
    }

}
export enum StarRatingColor {
    primary = "primary",
    accent = "accent",
    warn = "warn"
}
