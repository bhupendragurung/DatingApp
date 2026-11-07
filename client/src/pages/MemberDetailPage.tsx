import { Link, useLoaderData } from 'react-router'
import type { memberDetailLoader } from '../members/memberDetailLoader'

export default function MemberDetailPage() {
  const member = useLoaderData<typeof memberDetailLoader>()
  const memberSince = new Date(member.createdAt).toLocaleDateString(undefined, {
    month: 'long',
    year: 'numeric',
  })

  return (
    <article className="member-detail">
      <Link to="/members">← Back to members</Link>

      <header className="member-detail-header">
        <div className="member-avatar" aria-hidden="true">
          {member.knownAs.charAt(0).toUpperCase()}
        </div>
        <div>
          <h1>
            {member.knownAs}, {member.age}
          </h1>
          <p>
            {member.city}, {member.country} · Member since {memberSince}
          </p>
        </div>
      </header>

      <ProfileSection title="About" text={member.introduction} />
      <ProfileSection title="Looking for" text={member.lookingFor} />
      <ProfileSection title="Interests" text={member.interests} />
    </article>
  )
}

type ProfileSectionProps = { title: string; text: string | null }

function ProfileSection({ title, text }: ProfileSectionProps) {
  return (
    <section>
      <h2>{title}</h2>
      {text ? <p>{text}</p> : <p className="empty">Not filled in yet.</p>}
    </section>
  )
}