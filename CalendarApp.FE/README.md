# Calendar App build with Angular 19 - NgRx - MUI

Calendar app is a SPA app where you can add/update/remove events in a nice calendar UI

* Uses NgRx to manage the global state
* Material UI for styling and widget interaction
* Karma and Jasmine for Unit Testing

## About 

The calendar is a scheduling application that allows users to view, add, update, and delete calendar events.

## Running locally in development mode

To get started, just clone the repository and run `npm install && npm run dev`:

    git clone https://github.com/cristencean/calendar-events-app.git
    npm install
    npm run start
    
## Run the unit testing

To run the unit testing just type `npm run test`:

    npm run test

## Project structure

.
├── src/
│   ├── app/
│   │   ├── components/          # Reusable UI components
│   │   │   ├── error-label/     # Error label component
│   │   │   ├── calendar/        # Main calendar component
│   │   │   └── event-dialog/    # Event dialog component
│   │   ├── store/               # NgRx store (actions, reducers, selectors)
│   │   ├── shared/              # Shared reusable functionality
│   │   │   └── validators/      # Shared field validators
│   │   ├── services/            # Services (e.g., for API calls)
│   │   ├── app.module.ts        # Main Angular module
│   │   └── app.component.ts     # Root component
│   ├── assets/                  # Static assets (images, icons)
│   ├── environments/            # Environment configuration (dev, prod)
│   ├── styles.scss              # Global styles (SCSS)
│   ├── index.html               # Main HTML template
│   └── main.ts                  # Entry point for Angular
├── angular.json                 # Angular configuration
└── tsconfig.json                # TypeScript configuration

## Building and deploying in production

If you wanted to run this site in production, you should install modules then build the site with `npm run build`:

    npm install
    npm run build

You should run `npm run build` again any time you make changes to the site.