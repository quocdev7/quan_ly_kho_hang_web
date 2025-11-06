import { AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Inject, Input, OnDestroy, OnInit, Output, TemplateRef, ViewChild, ViewContainerRef, ViewEncapsulation } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { MatDialog } from '@angular/material/dialog';
import { MatDrawer } from '@angular/material/sidenav';
import dayGridPlugin from '@fullcalendar/daygrid';
import listPlugin from '@fullcalendar/list';
import interactionPlugin from '@fullcalendar/interaction';
import momentPlugin from '@fullcalendar/moment';
import rrulePlugin from '@fullcalendar/rrule';
import timeGridPlugin from '@fullcalendar/timegrid';
import { clone, cloneDeep, isEqual, omit } from 'lodash-es';
import { Calendar as FullCalendar } from '@fullcalendar/core';
import * as moment from 'moment';
import { RRule } from 'rrule';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { Calendar, CalendarDrawerMode, CalendarEvent, CalendarEventEditMode, CalendarEventPanelMode, CalendarSettings } from './calendar.types';
import allLocales from '@fullcalendar/core/locales-all'
import { CalendarRecurrenceComponent } from './recurrence/recurrence.component';
import { CalendarService } from './calendar.service';
import { TranslocoService } from '@ngneat/transloco';


@Component({
    selector: 'calendar',
    templateUrl: './calendar.component.html',
    styleUrls: ['./calendar.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None
})
export class CalendarComponent implements OnInit, AfterViewInit, OnDestroy {
    @Input() callbackFunction: (args: any) => void;
    @Input() filter: any;
    @Input() handleDrop: (args: any) => void;
    @Output() emitCalendarApi: EventEmitter<any> = new EventEmitter();
    @ViewChild('eventPanel') private _eventPanel: TemplateRef<any>;
    @ViewChild('fullCalendar') private _fullCalendar: FullCalendarComponent;
    @ViewChild('drawer') private _drawer: MatDrawer;
    calendars: Calendar[];
    calendarPlugins: any[] = [dayGridPlugin, interactionPlugin, listPlugin, momentPlugin, rrulePlugin, timeGridPlugin];
    drawerMode: CalendarDrawerMode = 'side';
    drawerOpened: boolean = true;
    stateDrawer: boolean = true;
    event: CalendarEvent;
    eventEditMode: CalendarEventEditMode = 'single';
    eventForm: FormGroup;
    eventTimeFormat: any;
    events: CalendarEvent[] = [];
    panelMode: CalendarEventPanelMode = 'view';
    settings: CalendarSettings;
    view: 'dayGridMonth' | 'timeGridWeek' | 'timeGridDay' | 'listYear' = 'timeGridWeek';
    views: any;
    viewTitle: string;
    public locales: any
    private _eventPanelOverlayRef: OverlayRef;
    private _fullCalendarApi: FullCalendar;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _calendarService: CalendarService,
        private _changeDetectorRef: ChangeDetectorRef,

        @Inject(DOCUMENT) private _document: Document,
        private _translocoSerive: TranslocoService,
        private _formBuilder: FormBuilder,
        private _matDialog: MatDialog,
        private _overlay: Overlay,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _viewContainerRef: ViewContainerRef
    ) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for event's recurrence status
     */
    get recurrenceStatus(): string {
        // Get the recurrence from event form
        const recurrence = this.eventForm.get('recurrence').value;

        // Return null, if there is no recurrence on the event
        if (!recurrence) {
            return null;
        }

        // Convert the recurrence rule to text
        let ruleText = RRule.fromString(recurrence).toText();
        ruleText = ruleText.charAt(0).toUpperCase() + ruleText.slice(1);

        // Return the rule text
        return ruleText;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {

        // Create the event form
        this.eventForm = this._formBuilder.group({
            id: [''],
            calendarId: [''],
            recurringEventId: [null],
            title: [''],
            description: [''],
            start: [null],
            end: [null],
            duration: [null],
            allDay: [true],
            recurrence: [null],
            range: [{}]
        });

        // Subscribe to 'range' field value changes
        this.eventForm.get('range').valueChanges.subscribe((value) => {

            if (!value) {
                return;
            }

            // Set the 'start' field value from the range
            this.eventForm.get('start').setValue(value.start, { emitEvent: false });
            // Set the end field
            this.eventForm.get('end').setValue(value.end, { emitEvent: false });

        });


        // Get calendars
        this._calendarService.calendars$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((calendars) => {

                // Store the calendars
                this.calendars = calendars;

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

        // Get events
        this._calendarService.events$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((events) => {

                // Clone the events to change the object reference so
                // that the FullCalendar can trigger a re-render.
                this.events = cloneDeep(events);

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

        // Get settings
        this._calendarService.settings$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((settings) => {

                // Store the settings
                this.settings = settings;

                // Set the FullCalendar event time format based on the time format setting
                this.eventTimeFormat = {
                    hour: settings.timeFormat === '12' ? 'numeric' : '2-digit',
                    hour12: settings.timeFormat === '12',
                    minute: '2-digit',

                    meridiem: settings.timeFormat === '12' ? 'short' : false
                };
                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {

                // Set the drawerMode and drawerOpened if the given breakpoint is active
                if (matchingAliases.includes('md')) {
                    this.drawerMode = 'side';
                    this.drawerOpened = true;
                }
                else {
                    this.drawerMode = 'over';
                    this.drawerOpened = false;
                }

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

        // Build the view specific FullCalendar options
        this.views = {
            dayGridMonth: {
                eventLimit: 6,
                eventTimeFormat: this.eventTimeFormat,
                fixedWeekCount: true,
            },
            timeGrid: {

                allDayText: '',
                columnHeaderFormat: {
                    weekday: 'short',
                    day: 'numeric',
                    omitCommas: true
                },
                columnHeaderHtml: (date): string => `<span class="fc-weekday">${this._translocoSerive.translate(moment(date).format('ddd'))}</span>
                                                       <span class="fc-date">${moment(date).format('D')}</span>`,
                slotDuration: '01:00:00',
                slotLabelFormat: this.eventTimeFormat
            },
            timeGridWeek: {

            },
            timeGridDay: {

            },
            listYear: {
                allDayText: 'All day',
                eventTimeFormat: this.eventTimeFormat,
                listDayFormat: true,
                listDayAltFormat: true
            },
        };
    }

    /**
     * After view init
     */
    ngAfterViewInit(): void {
        // Get the full calendar API
        this._fullCalendarApi = this._fullCalendar.getApi();
        // Get the current view's title
        this.viewTitle = this._fullCalendarApi.view.title;
        this.emitCalendarApi.emit(this._fullCalendarApi);
        this.locales = allLocales;
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();

        // Dispose the overlay
        if (this._eventPanelOverlayRef) {
            this._eventPanelOverlayRef.dispose();
        }
    }


    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Toggle Drawer
     */
    toggleDrawer(): void {
        // Toggle the drawer
        this._drawer.toggle();
        this.stateDrawer = this._drawer.opened;
    }

    /**
     * Open recurrence panel
     */
    openRecurrenceDialog(): void {
        // Open the dialog
        const dialogRef = this._matDialog.open(CalendarRecurrenceComponent, {
            panelClass: 'calendar-event-recurrence-dialog',
            data: {
                event: this.eventForm.value
            }
        });

        // After dialog closed
        dialogRef.afterClosed().subscribe((result) => {

            // Return if canceled
            if (!result || !result.recurrence) {
                return;
            }

            // Only update the recurrence if it actually changed
            if (this.eventForm.get('recurrence').value === result.recurrence) {
                return;
            }

            // If returned value is 'cleared'...
            if (result.recurrence === 'cleared') {
                // Clear the recurrence field if recurrence cleared
                this.eventForm.get('recurrence').setValue(null);
            }
            // Otherwise...
            else {
                // Update the recurrence field with the result
                this.eventForm.get('recurrence').setValue(result.recurrence);
            }
        });
    }



    /**
     * Get calendar by id
     *
     * @param id
     */
    getCalendar(id): Calendar {
        if (!id) {
            return;
        }
        return this.calendars.find(calendar => calendar.id === id);
    }

    /**
     * Change the calendar view
     *
     * @param view
     */
    changeView(view: 'dayGridMonth' | 'timeGridWeek' | 'timeGridDay' | 'listYear'): void {
        // Store the view
        this.view = view;

        // If the FullCalendar API is available...
        if (this._fullCalendarApi) {
            // Set the view
            this._fullCalendarApi.changeView(view);

            // Update the view title
            this.viewTitle = this._fullCalendarApi.view.title;
        }
    }

    /**
     * Moves the calendar one stop back
     */
    previous(): void {
        // Go to previous stop
        this._fullCalendarApi.prev();

        // Update the view title
        this.viewTitle = this._fullCalendarApi.view.title;

        // Get the view's current start date
        const start = moment(this._fullCalendarApi.view.currentStart);


    }

    /**
     * Moves the calendar to the current date
     */
    today(): void {
        // Go to today
        this._fullCalendarApi.today();

        // Update the view title
        this.viewTitle = this._fullCalendarApi.view.title;
    }

    /**
     * Moves the calendar one stop forward
     */
    next(): void {
        // Go to next stop
        this._fullCalendarApi.next();

        // Update the view title
        this.viewTitle = this._fullCalendarApi.view.title;

        // Get the view's current end date
        const end = moment(this._fullCalendarApi.view.currentEnd);
    }

    /**
     * On event click
     *
     * @param calendarEvent
     */
    onEventClick(calendarEvent): void {
        // Find the event with the clicked event's id
        const event: any = cloneDeep(this.events.find(item => item.db.id === calendarEvent.event._def.extendedProps.db.id));

        // Set the event
        this.event = event;

        // Prepare the end value
        let end;

        // If this is a recurring event...
        if (event?.recuringEventId) {
            // Calculate the end value using the duration
            end = moment(event.start).add(event.duration, 'minutes').toISOString();
        }
        // Otherwise...
        else {
            // Set the end value from the end
            end = event.end;
        }

        // Set the range on the event
        event.range = {
            start: event.start,
            end
        };

        // Reset the form and fill the event
        this.eventForm.reset();
        this.eventForm.patchValue(event);

        // Open the event panel
        this._openEventPanel(calendarEvent);
    }

    /**
     * On event render
     *
     * @param calendarEvent
     */
    onEventRender(calendarEvent): void {
        // Get event's calendar
        const calendar = this.calendars.find(item => item.id === calendarEvent.event.extendedProps.calendarId);

        // Return if the calendar doesn't exist...
        if (!calendar) {
            return;
        }

        // If current view is year list...
        if (this.view === 'listYear') {
            // Create a new 'fc-list-item-date' node
            const fcListItemDate1 = `<td class="fc-list-item-date">
                                            <span>
                                                <span>${moment(calendarEvent.event.start).format('D')}</span>
                                                <span>${this._translocoSerive.translate(moment(calendarEvent.event.start).format('MMM'))}, ${this._translocoSerive.translate(moment(calendarEvent.event.start).format('ddd'))}</span>
                                            </span>
                                        </td>`;

            // Insert the 'fc-list-item-date' into the calendar event element
            calendarEvent.el.insertAdjacentHTML('afterbegin', fcListItemDate1);

            // Set the color class of the event dot
            calendarEvent.el.getElementsByClassName('fc-event-dot')[0].classList.add(calendar.color);

            // Set the event's title to '(No title)' if event title is not available
            if (!calendarEvent.event.title) {
                calendarEvent.el.querySelector('.fc-list-item-title').innerText = '(No title)';
            }
        }
        // If current view is not month list...
        else {
            // Set the color class of the event
            calendarEvent.el.classList.add(calendar.color);

            // Set the event's title to '(No title)' if event title is not available
            if (!calendarEvent.event.title) {
                calendarEvent.el.querySelector('.fc-title').innerText = '(No title)';
            }
        }

        // Set the event's visibility
        calendarEvent.el.style.display = calendar.visible ? 'flex' : 'none';
    }


    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Create the event panel overlay
     *
     * @private
     */
    private _createEventPanelOverlay(positionStrategy): void {
        // Create the overlay
        this._eventPanelOverlayRef = this._overlay.create({
            panelClass: ['calendar-event-panel'],
            backdropClass: '',
            hasBackdrop: true,
            scrollStrategy: this._overlay.scrollStrategies.reposition(),
            positionStrategy
        });

        // Detach the overlay from the portal on backdrop click
        this._eventPanelOverlayRef.backdropClick().subscribe(() => {
            this._closeEventPanel();
        });
    }

    /**
     * Open the event panel
     *
     * @private
     */
    private _openEventPanel(calendarEvent): void {
        const positionStrategy = this._overlay.position().flexibleConnectedTo(calendarEvent.el).withFlexibleDimensions(false).withPositions([
            {
                originX: 'end',
                originY: 'top',
                overlayX: 'start',
                overlayY: 'top',
                offsetX: 8
            },
            {
                originX: 'start',
                originY: 'top',
                overlayX: 'end',
                overlayY: 'top',
                offsetX: -8
            },
            {
                originX: 'start',
                originY: 'bottom',
                overlayX: 'end',
                overlayY: 'bottom',
                offsetX: -8
            },
            {
                originX: 'end',
                originY: 'bottom',
                overlayX: 'start',
                overlayY: 'bottom',
                offsetX: 8
            }
        ]);

        // Create the overlay if it doesn't exist
        if (!this._eventPanelOverlayRef) {
            this._createEventPanelOverlay(positionStrategy);
        }
        // Otherwise, just update the position
        else {
            this._eventPanelOverlayRef.updatePositionStrategy(positionStrategy);
        }

        // Attach the portal to the overlay
        this._eventPanelOverlayRef.attach(new TemplatePortal(this._eventPanel, this._viewContainerRef));

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }
    /**
 * On calendar updated
 *
 * @param calendar
 */
    onCalendarUpdated(calendar): void {
        // Re-render the events
        this._fullCalendarApi.rerenderEvents();
    }
    /**
     * Close the event panel
     *
     * @private
     */
    private _closeEventPanel(): void {
        // Detach the overlay from the portal
        this._eventPanelOverlayRef.detach();

        // Reset the panel and event edit modes
        this.panelMode = 'view';
        this.eventEditMode = 'single';

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }



}
