import type {Member} from "./api/members";

type Props={member: Member }
export default function MemberCard({member}:Props){
    return (
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
  )
}