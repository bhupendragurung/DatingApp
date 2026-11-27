import type { LoaderFunctionArgs } from "react-router";
import { getMember } from "./api/members";
import { getLikedIds } from "../likes/api/likes";

export async function memberDetailLoader({ params }: LoaderFunctionArgs) {
  if (!params.id) {
    throw new Response('Member not found', { status: 404, statusText: 'Not Found' })
  }
  const [member, likedIds] = await Promise.all([getMember(params.id), getLikedIds()]);
  return { member, isLiked: likedIds.includes(member.id) };
}