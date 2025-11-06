import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { SharedModule } from 'app/shared/shared.module';
import { ChatComponent } from './chat.component';
import { ChatsComponent } from './chats/chats.component';
import { ContactInfoComponent } from './contact-info/contact-info.component';
import { ConversationComponent } from './conversation/conversation.component';
import { EmptyConversationComponent } from './empty-conversation/empty-conversation.component';
import { NewChatComponent } from './new-chat/new-chat.component';
import { ProfileComponent } from './profile/profile.component';
import { chatRoutes } from './chat.routing';
import { NewChatGroupComponent } from './new-group-chat/new-group-chat.component';
import {MatChipsModule} from '@angular/material/chips';
import {MatTabsModule} from '@angular/material/tabs'; 
import { popupAvatarGroupChatComponent } from './new-group-chat/popupAvatar.component';
import { popup_setting_chatComponent } from './setting-chat/popup-setting.component';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { LottieModule } from 'ngx-lottie';
//import { MentionModule } from 'angular-mentions';
import player from 'lottie-web';// by the AOT compiler.
import { sys_animation_chat_indexComponent } from './animation_chat/animation_options.component';
import { popupAvatarEditComponent } from './contact-info/popupAvatar.component';
import {  popupEditNameComponent } from './contact-info/popupEditName.component';
import {MatExpansionModule} from '@angular/material/expansion'; 
import { popupAddUserComponent } from './contact-info/popupAddUser.component';
import { popupNotificationComponent } from './contact-info/popupNotification.component';
import { MatRadioModule } from '@angular/material/radio';
import { ImageCropperModule } from 'ngx-image-cropper';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { SearchChatComponent } from './search/search.component';
import { popupBackgroundEditComponent } from './contact-info/popupBackground.component';
import { popupSendMessageComponent } from './contact-info/popupSendMessage.component';
import { MatTooltipModule } from "@angular/material/tooltip";

import { SweetAlert2Module } from "@sweetalert2/ngx-sweetalert2";
export function playerFactory() {
  return player;
}
@NgModule({
    declarations: [
        popupSendMessageComponent,
        popupBackgroundEditComponent,
        SearchChatComponent,
        popupAddUserComponent,
        popupNotificationComponent,
        popupEditNameComponent,
        popupAvatarEditComponent,
        sys_animation_chat_indexComponent,
        ChatComponent,
        ChatsComponent,
        ContactInfoComponent,
        ConversationComponent,
        EmptyConversationComponent,
        NewChatComponent,
        ProfileComponent,
        NewChatGroupComponent,
        popupAvatarGroupChatComponent,
        popup_setting_chatComponent,
    ],
    imports     : [
        ImageCropperModule,
        MatAutocompleteModule,
        MatButtonModule,
        MatTooltipModule,
        SweetAlert2Module,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        SharedModule,
        RouterModule.forChild(chatRoutes),
        LottieModule.forRoot({ player: playerFactory }),
        MatButtonModule,
        MatCheckboxModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        MatSidenavModule,
        SharedModule,
        MatChipsModule,
        MatTabsModule,
        MatSlideToggleModule,
        MatExpansionModule,
        MatRadioModule,
        MatCheckboxModule,
    ],
})
export class ChatModule
{
}
