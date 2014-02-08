using System;
using Newtonsoft.Json;

namespace test.fs_json.net.csobjects
{
    /// <summary>
    /// Base of all identity types
    /// </summary>
    [Serializable]
    public class BaseIdentity : IComparable<BaseIdentity>, IComparable
    {
        /// <summary>
        ///  Constructor for BaseIdentity 
        /// </summary>
        /// <param name="id">Guid id of the identity</param>
        public BaseIdentity(Guid id)
        {
            Id = id;
        }

        /// <summary>
        ///  Constructor for BaseIdentity 
        /// </summary>
        protected BaseIdentity()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        ///  Constructor for BaseIdentity 
        /// </summary>
        /// <param name="id">String Guid id of the identity. Will be turned into a Guid with Guid.Parse</param>
        protected BaseIdentity(string id)
            : this(Guid.Parse(id))
        {
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return Id.ToString();
        }

        /// <summary>
        /// The unique id of this identity
        /// NOTE: Must be a private setter to avoid problems with comparisons after it's set direct check test 'TestingComparisonOnUserId'
        /// </summary>
        [JsonProperty]
        public Guid Id { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="T:BaseIdentity"/> is equal to the current <see cref="T:BaseIdentity"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="other">The <see cref="T:BaseIdentity"/> to compare with the current <see cref="T:BaseIdentity"/>. </param><filterpriority>2</filterpriority>
        protected bool Equals(BaseIdentity other)
        {
            return Id.Equals(other.Id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((BaseIdentity)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(BaseIdentity other)
        {
            if (other.GetType() != GetType())
            {
                throw new ArgumentException("Identity is not the same type", "other");
            }

            return (other.Id.CompareTo(Id));
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj"/> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj"/>. Greater than zero This instance follows <paramref name="obj"/> in the sort order. 
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception><filterpriority>2</filterpriority>
        public int CompareTo(object obj)
        {
            if (obj.GetType() != GetType())
            {
                throw new ArgumentException("Object is not the same type", "obj");
            }
            return CompareTo(obj as BaseIdentity);
        }

#pragma warning disable 1591
        public static bool operator ==(BaseIdentity left, BaseIdentity right)
#pragma warning restore 1591
        {
            return Equals(left, right);
        }

#pragma warning disable 1591
        public static bool operator !=(BaseIdentity left, BaseIdentity right)
#pragma warning restore 1591
        {
            return !Equals(left, right);
        }
    }
}