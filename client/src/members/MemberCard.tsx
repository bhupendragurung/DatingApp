import { Link } from 'react-router'
import type { Member } from './api/members'

type Props = { member: Member }

export default function MemberCard({ member }: Props) {
  return (
    <Link to={`/members/${member.id}`} className="member-card-link">
      <article className="member-card">
        <div className="member-avatar" aria-hidden="true">
          {member.knownAs.charAt(0).toUpperCase()}
        </div>
        <h2>
          {member.knownAs}, {member.age}
        </h2>
        <p>
          {member.city}, {member.country}
        </p>
      </article>
    </Link>
  )
}