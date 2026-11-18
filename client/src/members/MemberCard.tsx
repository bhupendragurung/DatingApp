import { Link } from 'react-router'
import type { Member } from './api/members'
import { timeAgo } from './timeAgo'
type Props = { member: Member }

export default function MemberCard({ member }: Props) {
  return (
    <Link to={`/members/${member.id}`} className="member-card-link">
      <article className="member-card">
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
      </article>
    </Link>
  )
}