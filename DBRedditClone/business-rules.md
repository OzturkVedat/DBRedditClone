### 1. **Kullanıcı Yönetimi (Users)**

- Her kullanıcı, benzersiz bir `UserId` (UUID) ile tanımlanır.
- Her kullanıcı, benzersiz bir `UserName` ve `Email` ile sisteme kaydedilir.
- Kullanıcı şifreleri, `PasswordHash` alanında güvenli bir şekilde saklanır.
- Her kullanıcı, bir `Karma` değerine sahiptir; bu değer pozitif veya negatif olabilir.
- Bir kullanıcı yalnızca bir kez kaydedilebilir (benzersiz `UserName` ve `Email` kısıtlamaları).

### 2. **Rol Yönetimi (Roles ve RoleAssignments)**

- Her kullanıcıya bir veya birden fazla rol atanabilir.
- Rol tablosunda her rolün benzersiz bir `RoleId` ve `RoleName` değeri vardır.
- Kullanıcıların rolleri, `RoleAssignments` tablosunda kullanıcı kimliği ve rol kimliği ile eşleştirilir.
- Bir kullanıcı, yalnızca geçerli bir rol ile ilişkilendirilebilir.
- Kullanıcı ve rol silindiğinde, ilgili rol atamaları da silinir (CASCADE).

### 3. **Subreddit Yönetimi (Subreddits ve UserSubs)**

- Her subreddit, benzersiz bir `SubredditId` ile tanımlanır.
- Her subreddit, bir kullanıcı tarafından oluşturulabilir (`CreatedBy` alanı).
- Her subreddit bir ad ve isteğe bağlı bir açıklamaya sahip olabilir.
- Bir subreddit, kullanıcıları `UserSubs` tablosunda bağlayarak kullanıcılarla ilişkilendirilir.
- Bir subreddit silindiğinde, o subreddit ile ilişkilendirilen kullanıcılar da silinir (CASCADE).

### 4. **Gönderi Yönetimi (Posts ve PostReports)**

- Gönderiler, benzersiz bir `PostId` ile tanımlanır.
- Her gönderinin başlığı (`Title`) ve isteğe bağlı içeriği (`Content`) vardır.
- Gönderiler, kullanıcılar ve subredditler ile ilişkilendirilir.
- Gönderiler raporlanabilir. Raporlar, her gönderi ve her raporlama kullanıcısı için ayrı bir kayıt içerir.
- Raporlar, çözülüp çözülmediği bilgisini içerir ve raporlama kullanıcıları tarafından düzenlenebilir.
- Gönderi silindiğinde, ilgili raporlar da silinir (CASCADE).

### 5. **Etiket Yönetimi (Tags ve PostTags)**

- Gönderiler etiketlerle ilişkilendirilebilir.
- Her etiket, benzersiz bir `TagId` ve `TagName` ile tanımlanır.
- Etiketler, gönderilerle (`Posts`) `PostTags` tablosunda ilişkilendirilir.
- Gönderi veya etiket silindiğinde, ilişkilendirilmiş kayıtlar da silinir (CASCADE).

### 6. **Yorum Yönetimi (Comments ve CommentReactions)**

- Gönderilere yorum yapılabilir. Her yorum, benzersiz bir `CommentId` ile tanımlanır.
- Yorumlar, bir kullanıcıya ait olmalı ve isteğe bağlı olarak başka bir yorumu yanıtlayabilir (ParentId).
- Yorumlar, gönderilerle (`Posts`) ve kullanıcılarla (`Users`) ilişkilendirilir.
- Yorumlar, `CommentReactions` tablosunda kullanıcılar tarafından tepkiyle ilişkilendirilebilir.
- Yorumlar, çeşitli reaksiyonlarla (love, funny, sad, etc.) tepki alabilir.
- Yorum veya tepki silindiğinde, ilişkili diğer veriler de silinir (CASCADE).

### 7. **Mesajlaşma Sistemi (Chats ve Messages)**

- Kullanıcılar, diğer kullanıcılarla sohbetler başlatabilir.
- Her sohbet, bir `ChatId` ile tanımlanır ve her sohbet bir kullanıcıyla ilişkilidir.
- Sohbetler arasında mesajlaşma yapılabilir. Her mesaj, benzersiz bir `MessageId` ile tanımlanır.
- Mesajlar, gönderen kullanıcı ve sohbetle ilişkilendirilir.
- Bir sohbet veya mesaj silindiğinde, ilgili mesajlar da silinir (CASCADE).

### 8. **Oylama Sistemi (Votes, PostVotes ve CommentVotes)**

- Kullanıcılar, gönderiler ve yorumlar üzerinde oy kullanabilir.
- Oylama, yalnızca `+1` (upvote) veya `-1` (downvote) olarak yapılabilir (`VoteValue`).
- Oylama, kullanıcılar ve ilgili gönderi veya yorum ile ilişkilendirilir.
- Gönderi veya yorum silindiğinde, ilgili oylar da silinir (CASCADE).

### 9. **Veritabanı İlişkileri ve Silme Kuralları**

- Veritabanındaki tüm tablolar arasındaki ilişkiler, `FOREIGN KEY` kısıtlamaları ile belirlenmiştir.
- Ana verinin silinmesi (örneğin kullanıcı, subreddit, gönderi, vb.), ilişkili tüm verilerin (yorumlar, oylamalar, etiketler, vb.) otomatik olarak silinmesine (CASCADE) yol açar.
- Yorumlar arasında hiyerarşik ilişkiler mevcuttur. Yorumlar bir "ana" yorumu (ParentId) referans alabilir.
- Kullanıcıların `RoleAssignments`, `UserSubs`, `PostReports`, `PostVotes`, `CommentVotes`, `CommentReactions`, ve diğer ilişkili verileri otomatik olarak silinir.
