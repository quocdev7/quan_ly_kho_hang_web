

export interface user_firebase_model {
    count_chat_message_unread: number | null;
    count_notification: number | null;
    count_storege_byte: number | null;
    create_date_micro_epoch: string | null;
    device_name: string;
    devicetype: string;
    ios_voip: string;
    online: boolean | null;
    status_online: number | null;
    token_firebase: string;
    update_time_micro_epoch: string | null;
    user_id: string;
}

export interface vitual_model {
    date_sign_in: string | null;
    device_id: string;
    device_name: string;
    device_type: string;
    device_version: string;
    ios_voip: string;
    status: number | null;
    token_firebase: string;
}

export class chat_conversation_model {
    conversation_group_image: string;
    conversation_group_name: string;
    conversation_group_user_id_owner: string;
    count_message: number | null;
    count_total_file_size: number | null;
    create_time_micro_epoch: string | null;
    id: string;
    list_user_ids: string[];
    list_user_mute: string[];
    max_file_size: number | null;
    status_del: number | null;
    type: number | null;
    update_time_micro_epoch: string | null;
}

export class chat_conversation_message_model {
    conversation_id: string;
    extensionFile: string;
    fileName: string;
    filePath: string;
    fileSize: number | null;
    file_not_exit: string;
    id: string;
    is_forward: string;
    local_file_path: string;
    message: string;
    message_id: string;
    message_json: string;
    message_reply_id: string;
    progress_upload: string;
    reaction_count: number | null;
    reactions: string;
    send_time_micro_epoch: string | null;
    status: number | null;
    status_dell: number | null;
    type_message: number | null;
    update_time_micro_epoch: string | null;
    user_avatar_path: string;
    user_full_name: string;
    user_id: string;
}

export interface chat_conversation_message_reaction_model {
    update_time_micro_epoch: string | null;
    user_id: string;
    reaction_id: string;
    message_id: string;
    id: string;
}

export interface chat_conversation_user_info_model {
    update_time_micro_epoch: string | null;
    conversation_id: string;
    count_unread_message: number | null;
    last_seen_micro_epoch: string | null;
    id: string;
    user_id: string;
    mute_hours_notification: string;
    user_avatar_path: string;
    user_full_name: string;
}