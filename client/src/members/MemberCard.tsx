import { Link } from 'react-router'
import type { Member } from './api/members'
import { timeAgo } from './timeAgo'
import LikeButton from '../likes/LikeButton'

type Props = { member: Member; liked: boolean }

export default function MemberCard({ member, liked }: Props) {
  return (
    <article className="member-card">
      <Link to={`/members/${member.id}`} className="member-card-link">
        {member.photoUrl ? (
          <img
            src={member.photoUrl}
            alt={member.knownAs}
            className="member-avatar member-photo"
          />
        ) : (
          <div className="member-avatar" aria-hidden="true">
            {member.knownAs.charAt(0).toUpperCase()}
          </div>
        )}
        <h2>
          {member.knownAs}, {member.age}
        </h2>
        <p>
          {member.city}, {member.country}
        </p>
        <p className="last-active">Active {timeAgo(member.lastActive)}</p>
      </Link>
      <LikeButton targetUserId={member.id} liked={liked} />
    </article>
  )
}
