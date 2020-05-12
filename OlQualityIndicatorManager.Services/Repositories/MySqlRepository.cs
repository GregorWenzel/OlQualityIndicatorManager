using MySql.Data.MySqlClient;
using OlQualityIndicatorManager.Infrastructure.Domain;
using OlQualityIndicatorManager.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Services.Repositories
{
    public class MySqlRepository
    {
        List<OlSubsection> recommendationList = new List<OlSubsection>();

        MySqlConnection conn;

        public MySqlRepository()
        {
            string connString = "Server=localhost;Database=ol;User Id=ol_user;pwd=1nfinity!";
            conn = new MySqlConnection(connString);            
        }

        public void Open()
        {
            conn.Open();
        }

        public void Close()
        {
            conn.Close();
        }

        public string SafeGetString(MySqlDataReader reader, string colName)
        {
            int colIndex = reader.GetOrdinal(colName);

            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }

        public List<OlGuideline> GetGuidelines()
        {
            conn.Open();

            List<OlGuideline> result = new List<OlGuideline>();

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM guideline";
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    OlGuideline newGuideline = new OlGuideline();
                    newGuideline.IdKey = reader.GetInt32("guideline_id");
                    newGuideline.Uid = reader.GetString("uid");
                    newGuideline.Id = reader.GetString("id");
                    newGuideline.Title = reader.GetString("title");
                    newGuideline.ReviewDate = reader.GetDateTime("creation_date");
                    newGuideline.AwmfId = reader.GetString("awmf_id");
                    newGuideline.Version = reader.GetString("version");
                    newGuideline.Email = reader.GetString("email");
                    newGuideline.AddressDetails = reader.GetString("address_details");
                    newGuideline.ShortTitle = reader.GetString("short_title");

                    result.Add(newGuideline);
                }
            }
            reader.Close();

            foreach (OlGuideline guideline in result)
            {
                guideline.RecommendationList = GetRecommendationList(guideline);
                guideline.QualityIndicatorList = GetQualityIndicatorList(guideline);
            }
            
            conn.Close();

            return result;
        }

        public List<OlSubsection> GetRecommendationList(OlGuideline guideline)
        {
            List<OlSubsection> result = new List<OlSubsection>();

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM recommendation WHERE guideline_id = @gid";
            cmd.Parameters.AddWithValue("@gid", guideline.IdKey);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    OlSubsection recommendation = new OlSubsection();
                    recommendation.GuidelineId = guideline.IdKey;
                    recommendation.IdKey = reader.GetInt32("recommendation_id");
                    recommendation.Uid = SafeGetString(reader, "uid");
                    recommendation.Id = SafeGetString(reader, "id");
                    recommendation.TypePosition = reader.GetInt32("type_position");
                    recommendation.Position = reader.GetInt32("position");
                    recommendation.Number = SafeGetString(reader, "number");
                    recommendation.CreationDate = reader.GetDateTime("creation_date");
                    recommendation.EditState = new OlRecommendationEditState();
                    recommendation.EditState.Id = SafeGetString(reader, "edit_state");
                    recommendation.RecommendationGrade = new OlRecommendationGrade();
                    recommendation.RecommendationGrade.Id = SafeGetString(reader, "grade");
                    recommendation.TotalVote = SafeGetString(reader, "total_vote");
                    recommendation.RecommendationType = new OlRecommendationType();
                    recommendation.RecommendationType.Id = SafeGetString(reader, "type");
                    recommendation.Text = HelperFunctions.GetPlainTextFromHtml(reader.GetString("text"));
                    recommendation.IsExpertOpinion = reader.GetBoolean("is_expert_opinion");
                    recommendation.EditStateText = HelperFunctions.GetPlainTextFromHtml(SafeGetString(reader, "edit_state_text"));
                    recommendation.ParentSubsectionUid = SafeGetString(reader, "parent_subsection_uid");
                    result.Add(recommendation);
                }
            }
            reader.Close();
            return result;
        }

        public List<OlSubsection> GetQualityIndicatorList(OlGuideline guideline)
        {
            List<OlSubsection> result = new List<OlSubsection>();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM qualityindicator WHERE guideline_id = @gid";
            cmd.Parameters.AddWithValue("@gid", guideline.IdKey);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    OlSubsection qi = new OlSubsection();
                    qi.GuidelineId = guideline.IdKey;
                    qi.IdKey = reader.GetInt32("qualityindicator_id");
                    qi.Uid = SafeGetString(reader, "uid");
                    qi.Id = SafeGetString(reader, "id");
                    qi.TypePosition = reader.GetInt32("type_position");
                    qi.Position = reader.GetInt32("position");
                    qi.Numerator = SafeGetString(reader, "numerator");
                    qi.Denominator = SafeGetString(reader, "denominator");
                    qi.EvidenceBasis = SafeGetString(reader, "evidence_basis");
                    qi.Title = SafeGetString(reader, "title");
                    Enum.TryParse(SafeGetString(reader, "qi_type"), out OlQualityIndicatorType myType);
                    qi.IndicatorType = myType;
                    qi.Url = SafeGetString(reader, "url");
                    qi.UrlCertification = SafeGetString(reader, "url_certification");
                    qi.ParentSubsectionUid = SafeGetString(reader, "parent_subsection_uid");
                    result.Add(qi);
                }
            }
            reader.Close();

            foreach (OlSubsection qi in result)
            {
                qi.ReferenceRecommendationList = new List<OlSubsection>();
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM quality_indicator_has_recommendation WHERE qualityindicator_id = @qid";
                cmd.Parameters.AddWithValue("@qid", qi.IdKey);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OlSubsection refRecommendation = new OlSubsection();
                        refRecommendation = guideline.RecommendationList.First(item => item.IdKey == reader.GetInt32("recommendation_id"));
                        qi.ReferenceRecommendationList.Add(refRecommendation);
                    }
                }
                reader.Close();
            }
            
            return result;
        }

        public void SaveGuideline(OlGuideline guideline)
        {
            string cmdPrefix;
            string cmdSuffix;

            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT guideline_id FROM guideline WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", guideline.Id);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                guideline.IdKey = reader.GetInt32("guideline_id");
                cmdPrefix = "UPDATE guideline SET ";
                cmdSuffix = $" WHERE guideline_id = {guideline.IdKey}";
            }
            else
            {
                cmdPrefix = "INSERT INTO guideline SET ";
                cmdSuffix = string.Empty;
            }
            reader.Close();

            cmd = conn.CreateCommand();
            cmd.CommandText = cmdPrefix + "uid = @uid, id = @id, creation_date = @creation_date, title = @title, awmf_id = @awmf_id, version = @version, email = @email, address_details = @address_details, short_title = @short_title, state = @state, language = @lang";
            cmd.CommandText += cmdSuffix;
            cmd.Parameters.AddWithValue("@uid", guideline.Uid);
            cmd.Parameters.AddWithValue("@id", guideline.Id);
            cmd.Parameters.AddWithValue("@creation_date", guideline.ReviewDate);
            cmd.Parameters.AddWithValue("@awmf_id", guideline.AwmfId);
            cmd.Parameters.AddWithValue("@version", guideline.Version);
            cmd.Parameters.AddWithValue("@email", guideline.Email);
            cmd.Parameters.AddWithValue("@address_details", guideline.AddressDetails);
            cmd.Parameters.AddWithValue("@title", guideline.Title);
            cmd.Parameters.AddWithValue("@short_title", guideline.ShortTitle);
            cmd.Parameters.AddWithValue("@state", guideline.State);
            cmd.Parameters.AddWithValue("@lang", guideline.Language);

            cmd.ExecuteNonQuery();
            if (guideline.IdKey < 0)
            {
                guideline.IdKey = cmd.LastInsertedId;
            }

            SaveRecommendations(guideline);
            //SaveQualityIndicators(guideline);
            SaveWorkgroups(guideline);
            SaveAssociations(guideline);

            conn.Close();           
        }

        public void SaveRecommendations(OlGuideline guideline)
        {
            foreach (OlSubsection recommendation in guideline.RecommendationList)
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT recommendation_id FROM recommendation WHERE uid = @uid";
                cmd.Parameters.AddWithValue("@uid", recommendation.Uid);
                MySqlDataReader reader = cmd.ExecuteReader();             

                if (reader.HasRows)
                {
                    reader.Read();
                    recommendation.IdKey = reader.GetInt32("recommendation_id");
                }

                reader.Close();

                SaveRecommendation(recommendation, guideline.IdKey);
            }
        }


        public void SaveQualityIndicators(OlGuideline guideline)
        {
            foreach (OlSubsection qi in guideline.QualityIndicatorList)
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT qualityindicator_id FROM qualityindicator WHERE uid = @uid";
                cmd.Parameters.AddWithValue("@uid", qi.Uid);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    qi.IdKey = reader.GetInt32("qualityindicator_id");
                }
                reader.Close();

                SaveQualityIndicator(qi, guideline.IdKey);
            }
        }

        public void SaveAssociations(OlGuideline guideline)
        {
            foreach (OlInvolvedAssociation association in guideline.AssociationList)
            {
                string cmdPrefix;
                string cmdSuffix;

                if (!(association.Association is object)) continue;

                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT association_id FROM association WHERE uid = @uid";
                cmd.Parameters.AddWithValue("@uid", association.Association.Uid);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    association.Association.IdKey = reader.GetInt32("association_id");
                    cmdPrefix = "UPDATE association SET ";
                    cmdSuffix = $" WHERE association_id = {association.Association.IdKey}";
                }
                else
                {
                    cmdPrefix = "INSERT INTO association SET ";
                    cmdSuffix = string.Empty;
                }

                reader.Close();

                cmd = conn.CreateCommand();
                cmd.CommandText = cmdPrefix + "uid = @uid, id = @id, title = @title, url = @url, email = @email, abbreviation = @abbreviation, phone = @phone, street = @street, postal_code = @postal_code, place = @place, website_url = @website_url";
                cmd.CommandText += cmdSuffix;
                cmd.Parameters.AddWithValue("@uid", association.Association.Uid);
                cmd.Parameters.AddWithValue("@id", association.Association.Id);
                cmd.Parameters.AddWithValue("@title", association.Association.Title);
                cmd.Parameters.AddWithValue("@url", association.Association.Url);
                cmd.Parameters.AddWithValue("@email", association.Association.Email);
                cmd.Parameters.AddWithValue("@abbreviation", association.Association.Abbreviation);
                cmd.Parameters.AddWithValue("@phone", association.Association.Phone);
                cmd.Parameters.AddWithValue("@street", association.Association.Street);
                cmd.Parameters.AddWithValue("@postal_code", association.Association.PostalCode);
                cmd.Parameters.AddWithValue("@place", association.Association.Place);
                cmd.Parameters.AddWithValue("@website_url", association.Association.WebsiteUrl);
                cmd.ExecuteNonQuery();

                if (association.Association.IdKey < 0)
                {
                    association.Association.IdKey = cmd.LastInsertedId;
                }

                cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM association_has_author WHERE guideline_id = @gid AND association_id = @aid";
                cmd.Parameters.AddWithValue("@gid", guideline.IdKey);
                cmd.Parameters.AddWithValue("@aid", association.Association.IdKey);
                cmd.ExecuteNonQuery();

                foreach (OlAuthor author in association.AuthorList)
                {
                    SaveAuthor(author);
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO association_has_author SET guideline_id = @gid, association_id = @aid, author_id = @auid";
                    cmd.Parameters.AddWithValue("@gid", guideline.IdKey);
                    cmd.Parameters.AddWithValue("@aid", association.Association.IdKey);
                    cmd.Parameters.AddWithValue("@auid", author.IdKey);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveWorkgroups(OlGuideline guideline)
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM workgroup WHERE guideline_id = @gid";
            cmd.Parameters.AddWithValue("@gid", guideline.IdKey);
            cmd.ExecuteNonQuery();

            foreach (OlWorkgroup workgroup in guideline.WorkgroupList)
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO workgroup SET title = @title, guideline_id = @gid";
                cmd.Parameters.AddWithValue("@title", workgroup.Title);
                cmd.Parameters.AddWithValue("@gid", guideline.IdKey);
                cmd.ExecuteNonQuery();

                long workgroupId = cmd.LastInsertedId;

                cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM workgroup_has_author WHERE workgroup_id = @wid";
                cmd.Parameters.AddWithValue("@wid", workgroupId);
                cmd.ExecuteNonQuery();

                foreach (OlAuthor manager in workgroup.ManagerList)
                {
                    SaveAuthor(manager);

                    cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO workgroup_has_author SET workgroup_id = @wid, author_id = @aid, type = 'manager'";
                    cmd.Parameters.AddWithValue("@wid", workgroupId);
                    cmd.Parameters.AddWithValue("@aid", manager.IdKey);
                    cmd.ExecuteNonQuery();
                }

                foreach (OlAuthor member in workgroup.MemberList)
                {
                    SaveAuthor(member);

                    cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO workgroup_has_author SET workgroup_id = @wid, author_id = @aid, type = 'member'";
                    cmd.Parameters.AddWithValue("@wid", workgroupId);
                    cmd.Parameters.AddWithValue("@aid", member.IdKey);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveAuthor(OlAuthor author)
        {
            string cmdPrefix;
            string cmdSuffix;

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT author_id FROM author WHERE uid = @uid";
            cmd.Parameters.AddWithValue("@uid", author.Uid);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                author.IdKey = reader.GetInt32("author_id");
                cmdPrefix = "UPDATE author SET ";
                cmdSuffix = $" WHERE author_id = {author.IdKey}";
            }
            else
            {
                cmdPrefix = "INSERT INTO author SET ";
                cmdSuffix = string.Empty;
            }
            reader.Close();

            cmd = conn.CreateCommand();
            cmd.CommandText = cmdPrefix + "uid = @uid, id = @id, firstname = @firstname, lastname = @lastname, url = @url, post_title = @post_title, pre_title = @pre_title";
            cmd.CommandText += cmdSuffix;
            cmd.Parameters.AddWithValue("@uid", author.Uid);
            cmd.Parameters.AddWithValue("@id", author.Id);
            cmd.Parameters.AddWithValue("@firstname", author.FirstName);
            cmd.Parameters.AddWithValue("@lastname", author.LastName);
            cmd.Parameters.AddWithValue("@url", author.Url);
            cmd.Parameters.AddWithValue("@post_title", author.PostTitle);
            cmd.Parameters.AddWithValue("@pre_title", author.PreTitle);
            cmd.ExecuteNonQuery();

            if (author.IdKey < 0)
            {
                author.IdKey = cmd.LastInsertedId;
            }
        }

        public void SaveRecommendation(OlSubsection recommendation, long guideline_id)
        {
            string cmdPrefix;
            string cmdSuffix;

            if (recommendation.IdKey < 0)
            {
                cmdPrefix = "INSERT INTO recommendation SET ";
                cmdSuffix = string.Empty;
            }
            else
            {
                cmdPrefix = "UPDATE recommendation SET ";
                cmdSuffix = $" WHERE recommendation_id = {recommendation.IdKey}";
            }

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = cmdPrefix + "uid = @uid, id = @id, type_position = @type_position, position = @position, number = @number, guideline_id = @guideline_id, creation_date = @creation_date, edit_state = @edit_state, grade = @grade, total_vote = @total_vote, type = @type, text = @text, is_expert_opinion = @is_expert_opinion, edit_state_text = @edit_state_text, parent_subsection_uid = @puid";
            cmd.CommandText += cmdSuffix;
            cmd.Parameters.AddWithValue("@uid", recommendation.Uid);
            cmd.Parameters.AddWithValue("@id", recommendation.Id);
            cmd.Parameters.AddWithValue("@type_position", recommendation.TypePosition);
            cmd.Parameters.AddWithValue("@position", recommendation.Position);
            cmd.Parameters.AddWithValue("@number", recommendation.Number);
            cmd.Parameters.AddWithValue("@guideline_id", guideline_id);
            cmd.Parameters.AddWithValue("@creation_date", recommendation.CreationDate);
            cmd.Parameters.AddWithValue("@edit_state", recommendation.EditState.Id);
            cmd.Parameters.AddWithValue("@grade", recommendation.RecommendationGrade.Id);
            cmd.Parameters.AddWithValue("@total_vote", recommendation.TotalVote);
            cmd.Parameters.AddWithValue("@text", recommendation.Text);
            cmd.Parameters.AddWithValue("@type", recommendation.RecommendationType.Id);
            cmd.Parameters.AddWithValue("@is_expert_opinion", recommendation.IsExpertOpinion);
            cmd.Parameters.AddWithValue("@edit_state_text", recommendation.EditStateText);
            cmd.Parameters.AddWithValue("@puid", recommendation.ParentSubsectionUid);
            cmd.ExecuteNonQuery();

            if (recommendation.IdKey < 0)
            {
                recommendation.IdKey = cmd.LastInsertedId;
            }

            recommendationList.Add(recommendation);

            cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM level_of_evidence WHERE recommendation_id = @id";
            cmd.Parameters.AddWithValue("@id", recommendation.IdKey);
            cmd.ExecuteNonQuery();

            foreach (OlRecommendationLevelOfEvidence loe in recommendation.LevelOfEvidenceList)
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO level_of_evidence SET comment = @comment, recommendation_id = @id, name = @name";
                cmd.Parameters.AddWithValue("@id", recommendation.IdKey);
                cmd.Parameters.AddWithValue("@name", loe.Data.Id);
                cmd.Parameters.AddWithValue("@comment", loe.Comment);
                cmd.ExecuteNonQuery();
            }
        }

        public void SaveQualityIndicator(OlSubsection qi, long guideline_id)
        {
            string cmdPrefix;
            string cmdSuffix;

            if (qi.IdKey < 0)
            {
                cmdPrefix = "INSERT INTO qualityindicator SET ";
                cmdSuffix = string.Empty;
            }
            else
            {
                cmdPrefix = "UPDATE qualityindicator SET ";
                cmdSuffix = $" WHERE qualityindicator_id = {qi.IdKey}";
            }

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = cmdPrefix + "uid = @uid, id = @id, annotations = @annotations, type_position = @type_position, position = @position, guideline_id = @gid, numerator = @numerator, denominator = @denominator, title = @title, url = @url, evidence_basis = @evidence_basis, url_certification = @url_certification, parent_subsection_uid = @parent_subsection_uid";
            cmd.CommandText += cmdSuffix;
            cmd.Parameters.AddWithValue("@uid", qi.Uid);
            cmd.Parameters.AddWithValue("@id", qi.Id);
            cmd.Parameters.AddWithValue("@type_position", qi.TypePosition);
            cmd.Parameters.AddWithValue("@position", qi.Position);
            cmd.Parameters.AddWithValue("@gid", guideline_id);
            cmd.Parameters.AddWithValue("@title", qi.Title);
            //cmd.Parameters.AddWithValue("@qi_type", qi.IndicatorType.ToString());
            cmd.Parameters.AddWithValue("@numerator", qi.Numerator);
            cmd.Parameters.AddWithValue("@denominator", qi.Denominator);
            cmd.Parameters.AddWithValue("@url", qi.Url);
            cmd.Parameters.AddWithValue("@url_certification", qi.UrlCertification);
            cmd.Parameters.AddWithValue("@evidence_basis", qi.EvidenceBasis);
            cmd.Parameters.AddWithValue("@parent_subsection_uid", qi.ParentSubsectionUid);
            cmd.Parameters.AddWithValue("@annotations", qi.Annotations);
            cmd.ExecuteNonQuery();

            if (qi.IdKey < 0)
            {
                qi.IdKey = cmd.LastInsertedId;
            }

            cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM quality_indicator_has_recommendation WHERE qualityindicator_id = @uid";
            cmd.Parameters.AddWithValue("@uid", qi.IdKey);
            cmd.ExecuteNonQuery();

            foreach (OlSubsection recommendation in qi.ReferenceRecommendationList)
            {
                long recommendationId = recommendationList.First(item => item.Uid == recommendation.Uid).IdKey;
                cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO quality_indicator_has_recommendation SET qualityindicator_id = @qid, recommendation_id = @recommendation_id";
                cmd.Parameters.AddWithValue("@qid", qi.IdKey);
                cmd.Parameters.AddWithValue("@recommendation_id", recommendationId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
