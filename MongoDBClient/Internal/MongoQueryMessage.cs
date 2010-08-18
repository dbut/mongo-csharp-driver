﻿/* Copyright 2010 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using MongoDB.BsonLibrary;

namespace MongoDB.MongoDBClient.Internal {
    internal class MongoQueryMessage : MongoMessage {
        #region private fields
        private MongoCollection collection;
        private QueryFlags flags;
        private int numberToSkip;
        private int numberToReturn;
        private BsonDocument query;
        private BsonDocument fieldSelector;
        #endregion

        #region constructors
        internal MongoQueryMessage(
            MongoCollection collection,
            int numberToSkip,
            int numberToReturn,
            BsonDocument query,
            BsonDocument fieldSelector
        ) : 
            base(RequestOpCode.Query) {
            this.collection = collection;
            this.numberToSkip = numberToSkip;
            this.numberToReturn = numberToReturn;
            this.query = query;
            this.fieldSelector = fieldSelector;
        }
        #endregion

        #region protected methods
        protected override void WriteBodyTo(
            BinaryWriter writer
        ) {
            writer.Write((int) flags);
            WriteCString(writer, collection.FullName);
            writer.Write(numberToSkip);
            writer.Write(numberToReturn);

            BsonWriter bsonWriter = BsonBinaryWriter.Create(writer);
            if (query == null) {
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteEndDocument();
            } else {
                query.WriteTo(bsonWriter);
            }
            if (fieldSelector != null) {
                fieldSelector.WriteTo(bsonWriter);
            }
        }
        #endregion
    }
}
