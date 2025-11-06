export interface User
{
    id: string;
    name: string;
    email: string;
    avatar?: string;
    status?: string;
    type?: number;
    status_del?: number;
    host?:string;
}
