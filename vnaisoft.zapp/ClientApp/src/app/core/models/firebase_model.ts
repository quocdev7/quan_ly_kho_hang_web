export interface token_noti_user {
    id: string,
    token_firebase:string,
    domain: string,
    date_upDate:string,
    create_upDate:string,
    count_notification?:number,
    user_id:string,
    user_name:string,
}
export interface sub_device {
    date_sign_in: number,
    device_id: string,
    device_name: string,
    device_type: string,
    device_version: string,
    status:number,
    token_firebase: string,
}
