import { Route } from '@angular/router';
import { CalendarComponent } from './calendar.component';
import { CalendarSettingsComponent } from './settings/settings.component';
import { CalendarCalendarsResolver, CalendarSettingsResolver, CalendarWeekdaysResolver } from './calendar.resolvers';

export const calendarRoutes: Route[] = [
    //{
    //    path     : 'business_delivery_schedule_index',
    //    component: business_delivery_schedule_indexComponent,
    //    resolve  : {
    //        calendars: CalendarCalendarsResolver,
    //        settings : CalendarSettingsResolver,
    //        weekdays : CalendarWeekdaysResolver
    //    }
    //},
    //{
    //    path     : 'business_delivery_schedule_index',
    //    component: business_delivery_schedule_indexComponent,
    //    resolve  : {
    //        settings: CalendarSettingsResolver
    //    }
    //}
];
