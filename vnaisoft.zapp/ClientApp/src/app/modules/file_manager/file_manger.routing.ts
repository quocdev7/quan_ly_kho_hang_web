import { Route } from '@angular/router';


import { CalendarCalendarsResolver, CalendarSettingsResolver, CalendarWeekdaysResolver } from 'app/calendar/calendar.resolvers';
import { manager_folder_indexComponent } from './manager_folder/index.component';


export const storageRoutes: Route[] = [
    {
        path: "manager_folder_main/:id",
        component: manager_folder_indexComponent,
    },
];

