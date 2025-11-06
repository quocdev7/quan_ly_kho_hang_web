import { Route } from '@angular/router';
import { ChatComponent } from './chat.component';
import { ChatsComponent } from './chats/chats.component';
import { ConversationComponent } from './conversation/conversation.component';
import { EmptyConversationComponent } from './empty-conversation/empty-conversation.component';

export const chatRoutes: Route[] = [
    {
        path: '',
        component: ChatComponent,
        children: [
            {
                path: '',
                component: ChatsComponent,
                children: [
                    {
                        path: '',
                        pathMatch: 'full',
                        component: EmptyConversationComponent
                    },
                    {
                        path: 'message/:id',
                        component: ConversationComponent,
                    }
                ]
            }
        ]
    },
];
